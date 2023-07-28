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
                                          .Select(x => new { x.ProductId, x.Quantity })
                                          .ToList();

                var sellerQuantities = new Dictionary<int, int>();//這個字典的鍵 (Key) 是賣家的 ID (SellerId)，值 (Value) 是該賣家的合計數量。

                foreach (var productQuantity in productQuantities)
                {
                    int productId = (int)productQuantity.ProductId;
                    int quantity = (int)productQuantity.Quantity;

                    int sellerId = db.Products.FirstOrDefault(x => x.ProductId == productId)?.SellerId ?? 0;

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
                    SellerImagePath = db.Sellers.FirstOrDefault(x => x.SellerId == item.SellerId).SellerImagePath,
                    ShipmentStatusId = (int)item.ShipmentStatusId,
                    ShipmentStatus = db.ShipmentStatusDetails.FirstOrDefault(x => x.ShipmentStatusId == item.ShipmentStatusId).ShipmentStatus,
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