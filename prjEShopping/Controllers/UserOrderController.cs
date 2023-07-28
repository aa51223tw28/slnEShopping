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

            // 使用 GroupBy 和 Sum 來計算每個 SellerId 的商品數量總和
            var quantityBySeller = shipmentsForOrder.GroupBy(s => s.SellerId)
                                                    .Select(g => new{
                                                        SellerId = g.Key,
                                                        TotalQuantity = g.Sum(s => db.OrderDetails.FirstOrDefault(od => od.OrderId == s.OrderId)?.Quantity ?? 0)
                                                    }).ToList();

            // 建立一個 Dictionary 來存放每個 SellerId 對應的商品數量總和
            Dictionary<int, int> sellerQuantityMap = quantityBySeller.ToDictionary(x => x.SellerId, x => x.TotalQuantity);



            List<UserOrderAllVM> datas = new List<UserOrderAllVM>();
            foreach (var item in shipmentsForOrder)
            {
                
                var data = new UserOrderAllVM
                {
                    UserId = userid,
                    OrderId = (int)item.OrderId,
                    OrderNumber = db.Orders.FirstOrDefault(x => x.OrderId == item.OrderId).OrderNumber,
                    ShipmentId= item.ShipmentId,
                    ShipmentNumber=item.ShipmentNumber,
                    SellerId= (int)item.SellerId,
                    SellerName= db.Sellers.FirstOrDefault(x => x.SellerId == item.SellerId).SellerName,
                    Quantity=totalQuantity,
                    SellerImagePath= db.Sellers.FirstOrDefault(x=>x.SellerId== item.SellerId).SellerImagePath,
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