﻿using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class UserCartController : Controller
    {

        // GET: UserCart

        [Authorize]
        public ActionResult UserAddCartapi(int ProductId, int quantity)//傳過來買了什麼的api
        {
            var db = new AppDbContext();
            var customerAccount = User.Identity.Name;

            //找userid
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x=>x.UserId).FirstOrDefault();

            //新增一台購物車
            var cart = db.ShoppingCarts.FirstOrDefault(x => x.UserId == userid);
            //如果沒有購物車直接給他一台車
            if (cart == null)
            {
                var shoppingcart = new ShoppingCart()
                {
                    UserId = userid
                };
                db.ShoppingCarts.Add(shoppingcart);
                db.SaveChanges();
            }

            //取得新增資料加入table
            var cartid = db.ShoppingCarts.Where(x => x.UserId == userid).OrderByDescending(x=>x.CartId).Select(x => x.CartId).FirstOrDefault();
                        

            //確認是否已經加入過同一種商品
            var shoppingCartDetail = db.ShoppingCartDetails.FirstOrDefault(x => x.CartId == cartid && x.ProductId == ProductId);
            if (shoppingCartDetail == null)//沒加過
            {
                shoppingCartDetail = new ShoppingCartDetail()
                {
                    CartId= cartid,
                    ProductId= ProductId,
                    Quantity= quantity
                };
                db.ShoppingCartDetails.Add(shoppingCartDetail);
                
            }
            else
            {
                shoppingCartDetail.Quantity += quantity;
            }
            db.SaveChanges();

            return new EmptyResult();//這個Action方法返回一個空結果(EmptyResult)，表示操作已經完成，並不需要返回任何特定的內容或視圖。
        }
        public List<UserShoppingCartVM> datas { get; set; }

        [Authorize]
        public ActionResult UserShoppingCart()//購物車頁面
        {
            shoppingList();
            return View(datas);
        }       

        private void shoppingList()//秀UserShoppingCartVM的方法
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var cartid = db.ShoppingCarts.Where(x => x.UserId == userid).OrderByDescending(x => x.CartId).Select(x => x.CartId).FirstOrDefault();

            //應該與checkoutList()一模一樣但購物車頁面要將AddToOrder變換寫進去資料庫
            // 將 AddToOrder 的值為 null 的資料更新為 "0"
            var shoppingCartDetailsToUpdate = db.ShoppingCartDetails.Where(x => x.CartId == cartid).ToList();
            foreach (var item in shoppingCartDetailsToUpdate)
            {
                item.AddToOrder = "0";
            }
            db.SaveChanges();

            var data = db.ShoppingCartDetails.Where(x => x.CartId == cartid).OrderBy(x => x.CartDetailId)
                                .Join(db.Products, x => x.ProductId, y => y.ProductId, (x, y) => new
                                {
                                    CartId = x.CartId,
                                    UserId = userid,
                                    CartDetailId = x.CartDetailId,
                                    ProductId = x.ProductId,
                                    ProductName = y.ProductName,
                                    Quantity = x.Quantity,
                                    Price = y.Price,
                                    SubTotal = (x.Quantity) * (y.Price),
                                    ProductImagePathOne = y.ProductImagePathOne,
                                    SellerId = y.SellerId
                                }).ToList();

            datas = data.Select(x => new UserShoppingCartVM
            {
                CartId =(int) x.CartId,
                UserId = userid,
                CartDetailId = x.CartDetailId,
                ProductId = (int)x.ProductId,
                ProductName = x.ProductName,
                ProductImagePathOne = x.ProductImagePathOne,
                SellerId = (int)x.SellerId,
                SellerName = db.Sellers.FirstOrDefault(y => y.SellerId == x.SellerId).SellerName,
                ProductStock = calculateProductStock((int)x.ProductId, (int)x.Quantity),//計算庫存可不可以買

                Quantity = (int)x.Quantity,
                Price = (decimal)x.Price,
                Discount=db.ADProducts.FirstOrDefault(ad => ad.ProductId == x.ProductId)?.Discount ?? 0,
                //要判斷折價商品的時間
                DiscountPrice= IsInDiscountPeriod((int)x.ProductId)?(decimal)x.Price * (db.ADProducts.FirstOrDefault(ad => ad.ProductId == x.ProductId)?.Discount ?? 0)/100: (decimal)x.Price,
                SubTotal = (decimal)x.SubTotal,
                DiscountSubTotal= IsInDiscountPeriod((int)x.ProductId) ? (decimal)(x.Quantity)*((decimal)x.Price * (db.ADProducts.FirstOrDefault(ad => ad.ProductId == x.ProductId)?.Discount ?? 0) / 100): (decimal)x.SubTotal,
                
            }).ToList();

            //總金額
            //ViewBag.TotalPrice = datas.Sum(x => x.SubTotal);

        }

        private bool IsInDiscountPeriod(int productId)//判斷折價價格期限
        {
            var now = DateTime.Now;
            var db = new AppDbContext();
            return db.ADProducts.Any(ad => ad.ProductId == productId && ad.ADStartDate <= now && ad.ADEndDate >= now);
        }

        public int calculateProductStock(int productId,int quantity)//計算庫存的方法
        {
            var db=new AppDbContext();
            var orderQuantity = db.ProductStocks.Where(x => x.ProductId == productId).Select(x => x.OrderQuantity).FirstOrDefault() ?? 0;
            var stockQuantity = db.ProductStocks.Where(x => x.ProductId == productId).Select(x => x.StockQuantity).FirstOrDefault() ?? 0;
            var availableStock = stockQuantity - orderQuantity;
            int productStock = Math.Max(0, availableStock - quantity);
            return productStock;
        }

        public ActionResult GetTotalCount()//負責傳購買數量的api
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var cartid = db.ShoppingCarts.Where(x => x.UserId == userid).OrderByDescending(x => x.CartId).Select(x => x.CartId).FirstOrDefault();
            int totalCount=db.ShoppingCartDetails.Count(x=>x.CartId == cartid);
            return Json(totalCount, JsonRequestBehavior.AllowGet);
        }
        

        [Authorize]
        public ActionResult UserCheckout()//結帳頁面
        {
            checkoutList();
            return View(datas);
        }
        private void checkoutList()//秀UserShoppingCartVM的方法
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var cartid = db.ShoppingCarts.Where(x => x.UserId == userid).OrderByDescending(x => x.CartId).Select(x => x.CartId).FirstOrDefault();

            //應該與shoppingList()一模一樣但結帳頁要直接判斷x.AddToOrder=="1"
            var data = db.ShoppingCartDetails.Where(x => x.CartId == cartid&&x.AddToOrder=="1").OrderBy(x => x.CartDetailId)
                                .Join(db.Products, x => x.ProductId, y => y.ProductId, (x, y) => new
                                {
                                    CartId = x.CartId,
                                    UserId = userid,
                                    CartDetailId = x.CartDetailId,
                                    ProductId = x.ProductId,
                                    ProductName = y.ProductName,
                                    Quantity = x.Quantity,
                                    Price = y.Price,
                                    SubTotal = (x.Quantity) * (y.Price),
                                    ProductImagePathOne = y.ProductImagePathOne,
                                    SellerId = y.SellerId
                                }).ToList();

            datas = data.Select(x => new UserShoppingCartVM
            {
                CartId = (int)x.CartId,
                UserId = userid,
                CartDetailId = x.CartDetailId,
                ProductId = (int)x.ProductId,
                ProductName = x.ProductName,
                ProductImagePathOne = x.ProductImagePathOne,
                SellerId = (int)x.SellerId,
                SellerName = db.Sellers.FirstOrDefault(y => y.SellerId == x.SellerId).SellerName,
                ProductStock = calculateProductStock((int)x.ProductId, (int)x.Quantity),//計算庫存可不可以買

                Quantity = (int)x.Quantity,
                Price = (decimal)x.Price,
                Discount = db.ADProducts.FirstOrDefault(ad => ad.ProductId == x.ProductId)?.Discount ?? 0,
                //要判斷折價商品的時間
                DiscountPrice = IsInDiscountPeriod((int)x.ProductId) ? (decimal)x.Price * (db.ADProducts.FirstOrDefault(ad => ad.ProductId == x.ProductId)?.Discount ?? 0) / 100 : (decimal)x.Price,
                SubTotal = (decimal)x.SubTotal,
                DiscountSubTotal = IsInDiscountPeriod((int)x.ProductId) ? (decimal)(x.Quantity) * ((decimal)x.Price * (db.ADProducts.FirstOrDefault(ad => ad.ProductId == x.ProductId)?.Discount ?? 0) / 100) : (decimal)x.SubTotal,
            }).ToList();

                    
        }

        //[Authorize]
        //[HttpPost]
        //public ActionResult UserCheckout(UserShipmentDetailVM vm)不能用偷雞摸狗法只好一個一個傳UserCheckoutDataapi
        //{
        //    var customerAccount = User.Identity.Name;
        //    var db = new AppDbContext();
        //    var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();

        //    var shipmentNumber = db.Shipments.Where(x => x.OrderId == 15).Select(x => x.ShipmentNumber).ToList();
        //    foreach (var item in shipmentNumber)
        //    {
        //        var shipmentDetail = new ShipmentDetail()
        //        {
        //            ShipmentNumber = item,
        //            PaymentMethodId = db.PaymentMethods.FirstOrDefault(x => x.PaymentMethodName == vm.PaymentMethodName).PaymentMethodId,
        //            ShippingMethodId = db.ShippingMethods.FirstOrDefault(x => x.ShippingMethodName == vm.ShippingMethodName).ShippingMethodId,
        //            Receiver = vm.Receiver,
        //            ReceiverAddress = vm.ReceiverAddress,
        //        };
        //        db.ShipmentDetails.Add(shipmentDetail);
        //        db.SaveChanges();
        //    }

        //    return RedirectToAction("UserOrderDetailAll", "UserOrder");
        //}



        [Authorize]       
        public ActionResult UserCheckoutapi()//寫進資料庫
        {
            //這四行會一直重複待待之後優化
            var customerAccount = User.Identity.Name;
            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var cartid = db.ShoppingCarts.Where(x => x.UserId == userid).OrderByDescending(x => x.CartId).Select(x => x.CartId).FirstOrDefault();

            //在Orders的table新增訂單資料

            //取得下訂時間
            DateTime OrderDate = DateTime.Now;
            //訂單編號(日期+買家ID+訂單ID)：20230707+U01+001
            string formattedDate = OrderDate.ToString("yyyyMMdd");
           
            string formatteduserid = userid.ToString("D2");

            var orderIdold = db.Orders.OrderByDescending(x => x.OrderId).Select(x => x.OrderId).FirstOrDefault();
            string formattedOrderId = (orderIdold + 1).ToString("D3");//會自動補零保持3位
            
            var OrderNumber = formattedDate + "U" + formatteduserid + formattedOrderId;

            var datas = new Order()
            {
                UserId = userid,
                CouponId = 1,//先寫死
                OrderDate = OrderDate,
                OrderNumber = OrderNumber,
            };
            db.Orders.Add(datas);
            db.SaveChanges();


            //在OrderDetails的table新增訂單資料
            var shoppingdatas =db.ShoppingCartDetails.Where(x=>x.CartId== cartid && x.AddToOrder == "1").ToList();
            var orderId=db.Orders.Where(x=>x.UserId== userid).OrderByDescending(x=>x.OrderId).Select(x=>x.OrderId).FirstOrDefault();

            foreach (var item in shoppingdatas)
            {
                var product = db.Products.FirstOrDefault(x => x.ProductId == item.ProductId);
                var isDiscounted = IsInDiscountPeriod((int)item.ProductId);//如果有折扣商品判斷時間
                var discount = db.ADProducts.FirstOrDefault(ad => ad.ProductId == item.ProductId)?.Discount ?? 0;
                var currentPrice = isDiscounted ? (product.Price * discount/100) : product.Price;


                var orderDetail = new OrderDetail
                {
                    OrderId= orderId,
                    ProductId= item.ProductId,
                    Quantity = item.Quantity,
                    CurrentPrice= currentPrice,
                };
                db.OrderDetails.Add(orderDetail);
            }
            db.SaveChanges();

            //在Shipments的table新增訂單資料

            var orderdatas =db.OrderDetails.Where(x=>x.OrderId== orderId)
                                            .Join(db.Products, x => x.ProductId, y => y.ProductId, (x, y) => new
                                            {
                                                sellerid=y.SellerId,
                                                orderid=x.OrderId
                                            })
                                            .Distinct()
                                            .ToList();

            foreach (var item in orderdatas)
            {
                //出貨編號(訂單日期+賣家ID+訂單ID)：20230707+S01+001
                int sformattedsellerid = (int)item.sellerid; 
                int sformattedOrderId= (int)item.orderid;

                string ssformattedsellerid = sformattedsellerid.ToString("D2");
                string ssformattedOrderId= sformattedOrderId.ToString("D3");

                var ShipmentNumber = formattedDate + "S" + ssformattedsellerid + ssformattedOrderId;
                var shipment = new Shipment
                {                    
                    ShipmentNumber = ShipmentNumber,
                    OrderId = orderId,
                    SellerId= item.sellerid,
                    ShipmentDate= OrderDate,
                    ShipmentStatusId=1,//準備中
                };
                db.Shipments.Add(shipment);
            }
            db.SaveChanges ();

           

            //清空購物車--給一台新車
            var shoppingcart = new ShoppingCart()
            {
                UserId= userid
            };
            db.ShoppingCarts.Add(shoppingcart);
            db.SaveChanges ();

            //將上一台車如果還有未結帳的東西放到新車中
            var oldcartid= db.ShoppingCarts.Where(x=>x.UserId==userid).OrderByDescending(x=>x.CartId).Skip(1).Select(x=>x.CartId).FirstOrDefault();
            var oldShoppingCartDetails=db.ShoppingCartDetails.Where(x=>x.CartId== oldcartid&&x.AddToOrder=="0").ToList();
            
            var newcartid = db.ShoppingCarts.Where(x => x.UserId == userid).OrderByDescending(x => x.CartId).Select(x => x.CartId).FirstOrDefault();
          
            foreach (var item in oldShoppingCartDetails)
            {
                var newdata = new ShoppingCartDetail
                {
                    CartId= newcartid,
                    ProductId= item.ProductId,
                    Quantity= item.Quantity,
                    AddToOrder = item.AddToOrder
                };
                db.ShoppingCartDetails.Add(newdata);            
            }
            db.SaveChanges();


            //修改table ProductsStocks中的OrderQuantity
            var listproductid=db.OrderDetails.Where(x=>x.OrderId== orderId).Select(x=>x.ProductId).ToList();
            var productStocks = db.ProductStocks.Where(x => listproductid.Contains(x.ProductId));

            foreach (var item in productStocks)
            {
                var orderDetail = db.OrderDetails.FirstOrDefault(x => x.OrderId == orderId && x.ProductId == item.ProductId);

                if(orderDetail != null)
                {
                    int orderQuantity= (int)orderDetail.Quantity;
                    item.OrderQuantity += orderQuantity;
                    item.PurchaseQuantity -= orderQuantity;
                }
            }

            db.SaveChanges();

            return new EmptyResult();

        }
        [HttpPost]
        [Authorize]
        public ActionResult UserCheckoutDataapi(string shippingMethod, string paymentMethod, string receiverValue, string receiverAddressValue)//運送付款資訊的api
        {

            var customerAccount = User.Identity.Name;
            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var orderId = db.Orders.Where(x => x.UserId == userid).OrderByDescending(x => x.OrderId).Select(x => x.OrderId).FirstOrDefault();
            var shipmentNumber = db.Shipments.Where(x => x.OrderId == orderId).Select(x => x.ShipmentNumber).ToList();
           
            
            foreach (var item in shipmentNumber)
            {
                var shipmentDetail = new ShipmentDetail()
                {
                    ShipmentNumber = item,
                    PaymentMethodId = db.PaymentMethods.FirstOrDefault(x=>x.PaymentMethodName== paymentMethod).PaymentMethodId,
                    ShippingMethodId = db.ShippingMethods.FirstOrDefault(x=>x.ShippingMethodName== shippingMethod).ShippingMethodId,
                    Receiver = receiverValue,
                    ReceiverAddress = receiverAddressValue,
                };
                db.ShipmentDetails.Add(shipmentDetail);
                db.SaveChanges();
            }

            return new EmptyResult();
        }


        [Authorize]
        public ActionResult UserDeleteCartapi(int ProductId)//刪除購物車商品的api
        {
            var customerAccount = User.Identity.Name;
            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var cartid = db.ShoppingCarts.Where(x => x.UserId == userid).OrderByDescending(x => x.CartId).Select(x => x.CartId).FirstOrDefault();

            var cartItem = db.ShoppingCartDetails.FirstOrDefault(x => x.ProductId == ProductId&&x.CartId== cartid);
            if (cartItem == null) return new EmptyResult();

            var dbRemovecartDetailId = db.ShoppingCartDetails.Find(cartItem.CartDetailId);
            db.ShoppingCartDetails.Remove(dbRemovecartDetailId);
            db.SaveChanges();

            return new EmptyResult();
        }



        [Authorize]
        public ActionResult UserUpadateCartapi(int ProductId, int Quantity)//更新購物車商品數量的api
        {
            var customerAccount = User.Identity.Name;
            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var cartid = db.ShoppingCarts.Where(x => x.UserId == userid).OrderByDescending(x => x.CartId).Select(x => x.CartId).FirstOrDefault();

            var cartItem = db.ShoppingCartDetails.FirstOrDefault(x => x.ProductId == ProductId && x.CartId == cartid);
            if (cartItem == null) return new EmptyResult();            

            var cartItemDb = db.ShoppingCartDetails.FirstOrDefault(x => x.CartDetailId == cartItem.CartDetailId);
            cartItemDb.Quantity = Quantity;
            db.SaveChanges();

            return new EmptyResult();
        }


        [Authorize]
        public ActionResult UserUpadateCartStockapi(int ProductId)
        {
            //在購物車頁面按下+或加入購物車同時要Update ProductStocks table的PurchaseQuantity            
            //在UserShoppingCart頁面&UserSingleProduct頁面都要用到因為都是編輯購物車的商品數量
            
            var db = new AppDbContext();
            
            //直接去抓ShoppingCartDetails 裡面ProductId的Quantity合計
            var shoppingdetailquantity = db.ShoppingCartDetails.Where(x=>x.ProductId== ProductId).Sum(x => x.Quantity);

            //直接去抓OrderDetails 裡面ProductId的Quantity合計
            var orderDetails=db.OrderDetails.Where(x => x.ProductId == ProductId).Sum(x => x.Quantity);

            var purchaseQuantity = db.ProductStocks.FirstOrDefault(x => x.ProductId == ProductId);

            if (orderDetails == null && shoppingdetailquantity == null)
            {
                purchaseQuantity.PurchaseQuantity = 0;
            }
            else if (orderDetails == null && shoppingdetailquantity != null)
            {
                purchaseQuantity.PurchaseQuantity = shoppingdetailquantity;
            }
            else if (orderDetails != null && shoppingdetailquantity == null)
            {
                purchaseQuantity.PurchaseQuantity =  orderDetails;
            }
            else if(orderDetails != null && shoppingdetailquantity != null) 
            {
                purchaseQuantity.PurchaseQuantity = shoppingdetailquantity - orderDetails;
            }                
            db.SaveChanges();

            return new EmptyResult();
        }
        [Authorize]
        public ActionResult UserCartCheckStockapi(int cartid)//按下去買單要在檢核一次現在的訂單量
        {            
            var db = new AppDbContext();
            var cartDetails = db.ShoppingCartDetails.Where(x=>x.CartId== cartid).ToList();                                                  

            foreach (var cartDetail in cartDetails)
            {
                int productId = (int)cartDetail.ProductId;
                int cartQuantity=(int)cartDetail.Quantity;

                var productStock=db.ProductStocks.Where(x=>x.ProductId== productId).FirstOrDefault();
                                                    
                if (productStock != null)
                {
                    int stockQuantity = (int)productStock.StockQuantity;
                    int orderQuantity = (int)productStock.OrderQuantity;

                    int availableQuantity = stockQuantity - orderQuantity;
                    
                    if(cartQuantity> availableQuantity)//如果購物車數量>可以購買數量
                    {
                        cartDetail.Quantity = 0;
                        int newPurchaseQuantity = (int)(productStock.PurchaseQuantity - cartQuantity);
                        productStock.PurchaseQuantity = newPurchaseQuantity;
                        db.SaveChanges();
                        return Content("stock_insufficient");
                    }
                }
            }

            //確定所有商品的購買數量都不為0
            bool allQuantitiesNonZero = cartDetails.All(x => x.Quantity != 0);
            if (!allQuantitiesNonZero)
            {
                return Content("quantity_zero");

            }


            return Content("success");
        }


        [Authorize]
        public ActionResult getPaymentMethodName()
        {
            var db = new AppDbContext();
            var paymentMethodNames = db.PaymentMethods.Select(x => x.PaymentMethodName).Distinct();
            return Json(paymentMethodNames, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult getShippingMethodName()
        {
            var db = new AppDbContext();
            var shippingMethodNames = db.ShippingMethods.Select(x => x.ShippingMethodName).Distinct();
            return Json(shippingMethodNames, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        public ActionResult checkedBuyAllProductsapi()//全選
        {
            var customerAccount = User.Identity.Name;
            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var cartid = db.ShoppingCarts.Where(x => x.UserId == userid).OrderByDescending(x => x.CartId).Select(x => x.CartId).FirstOrDefault();

            //0是沒勾 1是有勾
            var shoppingdetails = db.ShoppingCartDetails.Where(x => x.CartId == cartid).ToList();

            //先檢查是否全都是1
            bool allItemsAreOne = shoppingdetails.All(x => x.AddToOrder == "1");

            foreach (var item in shoppingdetails)
            {
                if (allItemsAreOne)//如果全都是1才要變成0
                {
                    item.AddToOrder = "0";
                }
                else
                {
                    item.AddToOrder = "1";//如果有一個不是1是全部都要變成1
                }
            }


            db.SaveChanges();
            return new EmptyResult();
        }


        [Authorize]
        public ActionResult checkedBuyOneProductapi(int productId)//單勾選商品
        {
            var customerAccount = User.Identity.Name;
            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var cartid = db.ShoppingCarts.Where(x => x.UserId == userid).OrderByDescending(x => x.CartId).Select(x => x.CartId).FirstOrDefault();

            //0是沒勾 1是有勾
            var shoppingdetails = db.ShoppingCartDetails.Where(x => x.CartId == cartid&&x.ProductId== productId).FirstOrDefault();

            
            if (shoppingdetails.AddToOrder == "1")
            {
                shoppingdetails.AddToOrder = "0";
            }
            else
            {
                shoppingdetails.AddToOrder = "1";
            }


            db.SaveChanges();
            return new EmptyResult();
        }

        [Authorize]
        public ActionResult checkedBuyOneSellerapi(int sellerId)//勾選整個店家的商品
        {
            var customerAccount = User.Identity.Name;
            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var cartid = db.ShoppingCarts.Where(x => x.UserId == userid).OrderByDescending(x => x.CartId).Select(x => x.CartId).FirstOrDefault();

            var productids=db.Products.Where(x=>x.SellerId== sellerId).Select(x=>x.ProductId).ToList();

            var shoppingdetails = db.ShoppingCartDetails.Where(x => x.CartId == cartid && productids.Contains((int)x.ProductId));

            //先檢查是否全都是1
            bool allItemsAreOne = shoppingdetails.All(x => x.AddToOrder == "1");


            foreach (var item in shoppingdetails)
            {
                if (allItemsAreOne)
                {
                    item.AddToOrder = "0";
                }
                else
                {
                    item.AddToOrder = "1";
                }
            }

            db.SaveChanges();
            return new EmptyResult();
        }

        [Authorize]
        public ActionResult GetTotalcheckedCount()
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var cartid = db.ShoppingCarts.Where(x => x.UserId == userid).OrderByDescending(x => x.CartId).Select(x => x.CartId).FirstOrDefault();
            int totalcheckedCount = db.ShoppingCartDetails.Count(x => x.CartId == cartid && x.AddToOrder == "1");

            return Json(totalcheckedCount, JsonRequestBehavior.AllowGet);           
        }


        decimal grandTotal = 0;//總價=折扣+沒折扣


        public ActionResult GetTotalcheckedPrice()//總金額-有判斷是否有折扣UserCheckout UserShoppingCart都有呼叫此api
        {

            totalgrandTotal(0);
            return Json(grandTotal, JsonRequestBehavior.AllowGet);
        }

        private decimal totalgrandTotal(int sellerId)
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var cartid = db.ShoppingCarts.Where(x => x.UserId == userid).OrderByDescending(x => x.CartId).Select(x => x.CartId).FirstOrDefault();

            if (sellerId == 0)
            {
                var now = DateTime.Now;//確保特價商品期間有符合當下才能有折扣,否則就原價

                var discountproductids = db.ADProducts.Where(x => x.ADStartDate <= now && x.ADEndDate >= now)
                                                        .Select(x => x.ProductId)
                                                        .Distinct()
                                                        .ToList();

                var datatotal = db.ShoppingCartDetails.Where(x => x.CartId == cartid && x.AddToOrder == "1")
                                     .Join(db.Products, x => x.ProductId, y => y.ProductId, (x, y) => new
                                     {
                                         Quantity = x.Quantity,
                                         Price = y.Price,
                                         SubTotal = (x.Quantity) * (y.Price),
                                         ProductId = y.ProductId,
                                     })
                                     .Where(item => !discountproductids.Contains(item.ProductId))
                                     .ToList();
                var totalPrice = datatotal.Sum(x => x.SubTotal);//只有沒折扣商品的加總

                var datatotaldis = db.ShoppingCartDetails.Where(x => x.CartId == cartid && x.AddToOrder == "1")
                                        .Join(db.Products, x => x.ProductId, y => y.ProductId, (x, y) => new
                                        {
                                            Quantity = x.Quantity,
                                            Price = y.Price,
                                            Discount = db.ADProducts.FirstOrDefault(ad => ad.ProductId == x.ProductId).Discount,
                                            DiscountPrice = (y.Price) * (db.ADProducts.FirstOrDefault(ad => ad.ProductId == x.ProductId).Discount) / 100,
                                            DiscountSubTotal = ((y.Price) * (db.ADProducts.FirstOrDefault(ad => ad.ProductId == x.ProductId).Discount) / 100) * (x.Quantity),
                                            ProductId = y.ProductId,
                                        })
                                        .Where(item => discountproductids.Contains(item.ProductId))
                                        .ToList();
                var totalDiscountSubTotal = datatotaldis.Sum(x => x.DiscountSubTotal);//有折扣的加總


                grandTotal = (decimal)(totalPrice + totalDiscountSubTotal);
            }
            else
            {
                var now = DateTime.Now;//確保特價商品期間有符合當下才能有折扣,否則就原價

                var discountproductids = db.ADProducts.Where(x => x.ADStartDate <= now && x.ADEndDate >= now)
                                                        .Select(x => x.ProductId)
                                                        .Distinct()
                                                        .ToList();

                var datatotal = db.ShoppingCartDetails.Where(x => x.CartId == cartid && x.AddToOrder == "1")
                                     .Join(db.Products, x => x.ProductId, y => y.ProductId, (x, y) => new
                                     {
                                         Quantity = x.Quantity,
                                         Price = y.Price,
                                         SubTotal = (x.Quantity) * (y.Price),
                                         ProductId = y.ProductId,
                                         SellerId=y.SellerId,
                                     })
                                     .Where(item => !discountproductids.Contains(item.ProductId)&& item.SellerId == sellerId)//哪家商家的加總
                                     .ToList();
                var totalPrice = datatotal.Sum(x => x.SubTotal);//只有沒折扣商品的加總

                var datatotaldis = db.ShoppingCartDetails.Where(x => x.CartId == cartid && x.AddToOrder == "1")
                                        .Join(db.Products, x => x.ProductId, y => y.ProductId, (x, y) => new
                                        {
                                            Quantity = x.Quantity,
                                            Price = y.Price,
                                            Discount = db.ADProducts.FirstOrDefault(ad => ad.ProductId == x.ProductId).Discount,
                                            DiscountPrice = (y.Price) * (db.ADProducts.FirstOrDefault(ad => ad.ProductId == x.ProductId).Discount) / 100,
                                            DiscountSubTotal = ((y.Price) * (db.ADProducts.FirstOrDefault(ad => ad.ProductId == x.ProductId).Discount) / 100) * (x.Quantity),
                                            ProductId = y.ProductId,
                                            SellerId = y.SellerId,
                                        })
                                        .Where(item => discountproductids.Contains(item.ProductId) && item.SellerId == sellerId)//哪家商家的加總
                                        .ToList();
                var totalDiscountSubTotal = datatotaldis.Sum(x => x.DiscountSubTotal);//有折扣的加總


                grandTotal = (decimal)(totalPrice + totalDiscountSubTotal);
            }
            
            return grandTotal;
        }

        [Authorize]
        public ActionResult getCouponName()
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();

            var now = DateTime.Now;        

            var usercouponNames = db.UsersCoupons.Where(x => x.UserId == userid && x.CouponStatus == "可使用")
                                                .Join(db.Coupons, x => x.CouponId, y => y.CouponId, (x, y) => new
                                                {
                                                    couponId = x.CouponId,
                                                    couponName =y.CouponDetails,
                                                    StartTime = y.StartTime,
                                                    EndTime=y.EndTime,
                                                    CouponTerms=y.CouponTerms,
                                                    SellerId=y.SellerId,
                                                })
                                                .Where(x=>x.StartTime <= now && x.EndTime >= now)
                                                .ToList();
           

            return Json(usercouponNames, JsonRequestBehavior.AllowGet);
        }

       
       
        [Authorize]
        public ActionResult getshipPrice()//傳送運費
        {
            int shipPriceone = 60;//一家sellerid是60元

            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var cartid = db.ShoppingCarts.Where(x => x.UserId == userid).OrderByDescending(x => x.CartId).Select(x => x.CartId).FirstOrDefault();

            var sellerids = db.ShoppingCartDetails.Where(x => x.CartId == cartid && x.AddToOrder == "1")
                                                    .Join(db.Products, x => x.ProductId, y => y.ProductId, (x, y) => new
                                                    {
                                                        y.SellerId,
                                                    })
                                                    .Distinct()
                                                    .Count();
            int shipPrice = shipPriceone * sellerids;

            return Json(shipPrice, JsonRequestBehavior.AllowGet);
        }

        

        [Authorize]
        public ActionResult getcouponPrice(int couponId)//優惠券
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();

            int couponPrice = 0;
            if (couponId == 0)//是選到請選擇優惠券
            {
                couponPrice = 0;
            }

            var couponidselect=db.Coupons.Where(x=>x.CouponId== couponId).Select(x=>x.CouponId).FirstOrDefault();


            return Json(couponPrice, JsonRequestBehavior.AllowGet);
        }
    }
}
