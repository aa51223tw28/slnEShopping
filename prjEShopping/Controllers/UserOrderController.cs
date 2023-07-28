using prjEShopping.Models.EFModels;
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