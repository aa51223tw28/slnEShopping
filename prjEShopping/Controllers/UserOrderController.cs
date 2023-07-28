using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class UserOrderController : Controller
    {
        // GET: UserOrder

        [Authorize]
        public ActionResult UserOrderDetailAll()//訂單總覽頁面
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
                        
            var orderIdList = db.Orders.Where(x => x.UserId == userid).Select(x => x.OrderId).ToList(); 
            var shipmentsForOrder=db.Shipments.Where(x=> orderIdList.Contains((int)x.OrderId)).ToList();
                      
            
            List<UserOrderAllVM> datas = new List<UserOrderAllVM>();
            foreach (var item in shipmentsForOrder)
            {
                // 先找到該 OrderId 下的所有 ProductId
                var productIds = db.OrderDetails
                                   .Where(x => x.OrderId == item.OrderId)
                                   .Select(x => x.ProductId)
                                   .Distinct()
                                   .ToList();

                // 使用 GroupBy 和 Sum 來計算每個 SellerId 的商品數量總和
                int totalQuantity = (int)productIds.Sum(productId => db.OrderDetails
                                                                    .Where(x => x.OrderId == item.OrderId && x.ProductId == productId)
                                                                    .Sum(x => x.Quantity));

                // 根據 ProductId 找到對應的 SellerId
                var sellerId = db.Products.FirstOrDefault(p => p.ProductId == productIds.FirstOrDefault())?.SellerId;


                var data = new UserOrderAllVM
                {
                    UserId = userid,
                    OrderId = (int)item.OrderId,
                    OrderNumber = db.Orders.FirstOrDefault(x => x.OrderId == item.OrderId).OrderNumber,
                    ShipmentId = item.ShipmentId,
                    ShipmentNumber = item.ShipmentNumber,
                    SellerId = (int)item.SellerId,
                    SellerName = db.Sellers.FirstOrDefault(x => x.SellerId == item.SellerId).SellerName,
                    
                    Quantity = totalQuantity,
                    SellerImagePath = db.Sellers.FirstOrDefault(x=>x.SellerId== item.SellerId).SellerImagePath,
                    ShipmentStatusId = (int)item.ShipmentStatusId,
                };
                datas.Add(data);
            }
            return View(datas);
        }


        [Authorize]
        public ActionResult UserOrderDetail()//訂單詳細頁面
        {
            return View();
        }


        [Authorize]
        public ActionResult UserShipmentDetail()//出貨單單詳情頁面
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var orderid = db.Orders.Where(x => x.UserId == userid).OrderByDescending(x => x.OrderId).Select(x => x.OrderId).FirstOrDefault();

            var orderNumber = db.Orders.Where(x => x.OrderId == orderid).Select(x => x.OrderNumber).FirstOrDefault();


            var data = db.OrderDetails.Where(x => x.OrderId == orderid).OrderBy(x => x.OrderDetailId)
                .Join(db.Products, x => x.ProductId, y => y.ProductId, (x, y) => new
                {
                    ProductName = y.ProductName,
                    Quantity = x.Quantity,
                    CurrentSubTotal = (x.CurrentPrice) * (x.Quantity),
                    ProductImagePathOne = y.ProductImagePathOne,
                    OrderNumber = orderNumber,

                }).ToList();

            return View();
        }

    }
}