using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class UserOrderController : Controller
    {
        // GET: UserOrder
        //這個很像productlist singleproductlist要傳productid
        [Authorize]
        public ActionResult UserOrderDetailAll()//訂單總覽頁面
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();

            var orderIdList = db.Orders.Where(x => x.UserId == userid).Select(x => x.OrderId).ToList();
            var shipmentsForOrder = db.Shipments.Where(x => orderIdList.Contains((int)x.OrderId)).ToList();

            List<UserOrderAllVM> datas = new List<UserOrderAllVM>();
            foreach (var item in shipmentsForOrder)
            {

                //---計算該筆OrderId所買的商品ProductId該SellerId的合計Quantity
                var orderDetailIds = db.OrderDetails
                                        .Where(x => x.OrderId == item.OrderId)
                                        .Select(x => x.OrderDetailId)
                                        .ToList();

                var productQuantities = db.OrderDetails
                                          .Where(x => orderDetailIds.Contains(x.OrderDetailId))
                                          .Select(x => new { x.ProductId, x.Quantity,x.CurrentPrice })
                                          .ToList();

                var sellerQuantities = new Dictionary<int, int>();//這個字典的鍵 (Key) 是賣家的 ID (SellerId)，值 (Value) 是該賣家的合計數量。

                foreach (var productQuantity in productQuantities)//查詢所有訂單明細OrderDetails
                {
                    int productId = (int)productQuantity.ProductId;
                    int quantity = (int)productQuantity.Quantity;

                    int sellerId = db.Products.FirstOrDefault(x => x.ProductId == productId)?.SellerId ?? 0;//?.如果不為空取出SellerId

                    if (sellerQuantities.ContainsKey(sellerId))
                    {
                        sellerQuantities[sellerId] += quantity;
                    }
                    else
                    {
                        sellerQuantities[sellerId] = quantity;
                    }
                }
                //---計算該筆OrderId所買的商品ProductId該SellerId的合計Quantity

                

                var data = new UserOrderAllVM
                {
                    UserId = userid,
                    OrderId = (int)item.OrderId,
                    OrderNumber = db.Orders.FirstOrDefault(x => x.OrderId == item.OrderId).OrderNumber,
                    ShipmentId = item.ShipmentId,
                    ShipmentNumber = item.ShipmentNumber,
                    SellerId = (int)item.SellerId,
                    SellerName = db.Sellers.FirstOrDefault(x => x.SellerId == item.SellerId).SellerName,
                    Quantity = sellerQuantities.ContainsKey((int)item.SellerId) ? sellerQuantities[(int)item.SellerId] : 0,                    
                    PriceBySeller= (decimal)productQuantities.Where(x => db.Products.FirstOrDefault(p => p.ProductId == x.ProductId)?.SellerId == (int)item.SellerId).Sum(x => x.CurrentPrice * x.Quantity),
                    SellerImagePath = db.Sellers.FirstOrDefault(x => x.SellerId == item.SellerId).SellerImagePath,
                    ShipmentStatusId = (int)item.ShipmentStatusId,
                    ShipmentStatus = db.ShipmentStatusDetails.FirstOrDefault(x => x.ShipmentStatusId == item.ShipmentStatusId).ShipmentStatus,
                };

                datas.Add(data);
            }

            return View(datas);

        }

        [Authorize]
        public ActionResult UserOrderDetail(int orderId)//訂單詳細頁面
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            
            var data =db.OrderDetails.Where(x=>x.OrderId== orderId).OrderBy(x=>x.OrderDetailId)
                                        .Join(db.Products, x => x.ProductId, y => y.ProductId, (x, y) => new UserOrderAllVM
                                        {
                                            OrderId=orderId,
                                            OrderNumber=db.Orders.FirstOrDefault(o=>o.OrderId== orderId).OrderNumber,
                                            SellerId=(int)y.SellerId,
                                            SellerName=db.Sellers.FirstOrDefault(s=>s.SellerId== y.SellerId).SellerName,
                                            ProductImagePathOne=y.ProductImagePathOne,
                                            ProductId =y.ProductId,
                                            ProductName=y.ProductName,
                                            QuantityByProduct= (int)x.Quantity,
                                            CurrentPrice= (int)x.CurrentPrice,
                                            SubTotal= (decimal)((decimal)(x.Quantity)*(x.CurrentPrice))
                                        }).ToList();

            // 計算總金額
            ViewBag.TotalPrice = data.Sum(x => x.SubTotal);           
            return View(data);
        }


        [Authorize]
        public ActionResult UserShipmentDetail(int shipmentId,int sellerid)//出貨單單詳情頁面
        {
            var customerAccount = User.Identity.Name;
            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();

            var shipment = db.Shipments.FirstOrDefault(x => x.ShipmentId == shipmentId);
            var orderidlist = db.Shipments.Where(x => x.ShipmentId == shipmentId).Select(x => x.OrderId).ToList();
            var productidlist = db.OrderDetails.Join(db.Products, x => x.ProductId, y => y.ProductId, (x, y) => new
            {
                orderid = x.OrderId,
                productId = x.ProductId,
                sellerid = y.SellerId,
                quantity = x.Quantity,
                currentprice = x.CurrentPrice,                
            }).Where(x => orderidlist.Contains(x.orderid)&&x.sellerid== sellerid).ToList();


            List<UserOrderAllVM> datas = new List<UserOrderAllVM>();

            foreach (var item in productidlist)
            {
                var data = new UserOrderAllVM
                {
                    OrderId = (int)item.orderid,
                    OrderNumber = db.Orders.FirstOrDefault(x => x.OrderId == item.orderid).OrderNumber,
                    ShipmentId = shipmentId,
                    ShipmentNumber = db.Shipments.FirstOrDefault(x => x.ShipmentId == shipmentId).ShipmentNumber,
                    ProductId = (int)item.productId,
                    ProductName= db.Products.FirstOrDefault(x=>x.ProductId== item.productId).ProductName,
                    Quantity=(int)item.quantity,
                    CurrentPrice=(decimal) item.currentprice,
                    SubTotal= (decimal)((decimal)(item.quantity)*(item.currentprice)),
                    ProductImagePathOne= db.Products.FirstOrDefault(x=>x.ProductId == item.productId).ProductImagePathOne,
                    ShipmentDate = shipment?.ShipmentDate ?? DateTime.MinValue,
                    ShipDate = shipment?.ShipDate ?? DateTime.MinValue,
                    ArrivalTimeDate = shipment?.ArrivalTimeDate ?? DateTime.MinValue,
                    PickDate = shipment?.PickDate ?? DateTime.MinValue,
                    CompletionDate = shipment?.CompletionDate ?? DateTime.MinValue,

                };
                datas.Add(data);
            }

            return View(datas);        

        }



        [Authorize]
        public ActionResult UserShipmentDemoPickapi(int shipmentid)//demo 領貨api
        {
            var customerAccount = User.Identity.Name;
            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();

            var date= DateTime.Now;
            var pickdata=db.Shipments.FirstOrDefault(x=>x.ShipmentId== shipmentid);
            pickdata.PickDate = date;
            pickdata.ShipmentStatusId = 4;
            db.SaveChanges();

            return new EmptyResult();
        }

        public ActionResult UserRating(int ShipmentDetailId)
        {
            using (var db = new AppDbContext())
            {
                var productViewModel = db.Products
            .Where(p => p.ProductId == ShipmentDetailId) // Use the correct property and condition
            .Select(p => new ProductViewModel // Use the correct ViewModel type
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                // Map other properties
            })
            .FirstOrDefault();

                var shipmentViewModel = db.ShipmentDetails
             .Where(s => s.ShipmentDetailId == ShipmentDetailId)
             .Select(s => new ShipmentViewModel
             {
                 ShipmentDetailId = s.ShipmentDetailId,
                 ShipmentNumber = s.ShipmentNumber,
                 // Map other properties
             })
             .FirstOrDefault();

                // Pass the view models to the view
                var combinedViewModel = new CombinedVM
                {
                    ProductViewModel = productViewModel,
                    ShipmentViewModel = shipmentViewModel
                };

                return View(combinedViewModel);
            }
        }


        [Authorize]
        public ActionResult getshipPriceorder(int orderId)//傳送運費
        {
            int shipPriceone = 60;//一家sellerid是60元

            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            
            var sellerids = db.OrderDetails.Where(x => x.OrderId == orderId)
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
        public ActionResult getcouponPriceorder(int orderId)//傳送優惠券
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();

            var sellerids = db.OrderDetails.Where(x => x.OrderId == orderId)
                                           .Join(db.Products, x => x.ProductId, y => y.ProductId, (x, y) => new
                                           {
                                               y.SellerId,
                                           })
                                           .Distinct()
                                           .Count();

            var couponid = db.Orders.Where(x => x.OrderId == orderId).Select(x => x.CouponId).FirstOrDefault();
            var couponprice = 0;
            var couponidselect = db.Coupons.Where(x => x.CouponId == couponid).FirstOrDefault();
            if (couponidselect.CouponType == "抵用券")
            {
                couponprice = int.Parse(couponidselect.Discount);
            }
            else if (couponidselect.CouponType == "免運券" && couponidselect.SellerId == 0)
            {
                couponprice = int.Parse(couponidselect.Discount) * sellerids;
            }
            else if (couponidselect.CouponType == "免運券" && couponidselect.SellerId != 0)
            {
                couponprice = int.Parse(couponidselect.Discount);
            }
            else if (couponidselect.CouponType == "折價券")
            {
                var selleridcoupon = couponidselect.SellerId;

                if (selleridcoupon == 0)
                {
                    var subtotal = db.OrderDetails.Where(x => x.OrderId == orderId)
                                                .Sum(x => x.CurrentPrice * x.Quantity);
                    var discount = decimal.Parse(couponidselect.Discount) / 100;
                    couponprice = (int)(decimal)(subtotal * (1 - discount));
                }
                else
                {
                    var subtotal = db.OrderDetails.Where(x => x.OrderId == orderId)
                                                .Join(db.Products, x => x.ProductId, y => y.ProductId, (x, y) => new
                                                {
                                                    CurrentPrice = x.CurrentPrice,
                                                    Quantity = x.Quantity,
                                                    y.SellerId,
                                                })
                                                .Where(x => x.SellerId == selleridcoupon)
                                                .Sum(x => x.CurrentPrice * x.Quantity);
                    var discount = decimal.Parse(couponidselect.Discount) / 100;
                    couponprice = (int)(decimal)(subtotal * (1 - discount));
                }


            }

            return Json(couponprice, JsonRequestBehavior.AllowGet);
        }
    }

}
