using prjEShopping.Models.DTOs;
using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class UserCartController : Controller
    {

        // GET: UserCart

        [Authorize]
        public ActionResult UserAddCart(int ProductId, int quantity)//傳過來買了什麼的api
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

        private void shoppingList()//UserShoppingCartVM的方法
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var cartid = db.ShoppingCarts.Where(x => x.UserId == userid).OrderByDescending(x => x.CartId).Select(x => x.CartId).FirstOrDefault();

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
                CartId = (int)x.CartId,
                UserId = userid,
                CartDetailId = x.CartDetailId,
                ProductId = (int)x.ProductId,
                ProductName = x.ProductName,
                Quantity = (int)x.Quantity,
                Price = (decimal)x.Price,
                SubTotal = (decimal)x.SubTotal,
                ProductImagePathOne = x.ProductImagePathOne,
                SellerId = (int)x.SellerId
            }).ToList();

            //總金額
            ViewBag.TotalPrice = datas.Sum(x => x.SubTotal);
        }

        public ActionResult GetTotalCount()//負責傳購買數量的api
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var cartid = db.ShoppingCarts.Where(x => x.UserId == userid).Select(x => x.CartId).FirstOrDefault();
            int totalCount=db.ShoppingCartDetails.Count(x=>x.CartId == cartid);
            return Json(totalCount, JsonRequestBehavior.AllowGet);
        }
        

        [Authorize]
        public ActionResult UserCheckout()//結帳頁面
        {
            shoppingList();
            return View(datas);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UserCheckout(UserCheckoutVM vm)//寫進資料庫
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
            var shoppingdatas =db.ShoppingCartDetails.Where(x=>x.CartId== cartid).ToList();
            var orderId=db.Orders.Where(x=>x.UserId== userid).OrderByDescending(x=>x.OrderId).Select(x=>x.OrderId).FirstOrDefault();

            foreach (var item in shoppingdatas)
            {
                var product = db.Products.FirstOrDefault(x => x.ProductId == item.ProductId);

                var orderDetail = new OrderDetail
                {
                    OrderId= orderId,
                    ProductId= item.ProductId,
                    Quantity = item.Quantity,
                    CurrentPrice= product.Price
                };
                db.OrderDetails.Add(orderDetail);
            }
            db.SaveChanges();

            //在Shipments的table新增訂單資料






            //清空購物車--給一台新車



            return View("UserOrderDetail");
        }


        [Authorize]
        public ActionResult UserOrderDetail()//訂單詳情頁面
        {
            return View();
        }
    }
 }
