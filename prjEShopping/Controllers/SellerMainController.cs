using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class SellerMainController : Controller
    {
        // GET: SellerMain
        public ActionResult Index()
        {
            var customerAccount = User.Identity.Name;
            int id = (int)Session["SellerId"];
            // 根據賣家的 ID 從資料庫中讀取賣家資料
            var db = new AppDbContext();
            var seller = db.Sellers.FirstOrDefault(x => x.SellerId == id);

            var sellerid = db.Sellers.Where(x => x.SellerAccount == customerAccount).Select(x => x.SellerId).FirstOrDefault();

            var datas = db.Sellers.Where(x => x.SellerId == sellerid).Select(x => new SellerRegisterVM()
            {
                SellerName = x.SellerName,
                SellerAccount = x.SellerAccount,
                Phone = x.Phone
            }).FirstOrDefault();
            // 將賣家資料傳遞給 SellerMain 頁面
            return View(seller);
        }

        public ActionResult LoadCoupons() 
        {
            int id = (int)Session["SellerId"];
            var db = new AppDbContext();
            var coupons = db.Coupons.Where(x => x.SellerId == id && x.EndTime > DateTime.Now).OrderBy(x => x.EndTime).Take(3);
            return Json(coupons,JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTop5Product()
        {
            int sellerid = (int)Session["SellerId"];
            var db = new AppDbContext();
            var products = db.Products.Where(x => x.SellerId == sellerid && x.ProductStatusId == 2).Join(db.ProductStocks, x => x.ProductId, y => y.ProductId, (x, y) => new
            {
                x.ProductId,
                x.ProductImagePathOne,
                x.ProductName,
                x.Price,
                y.QuantitySold,
                x.SubcategoryId,
            }).OrderByDescending(x => x.QuantitySold).Take(5);

            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getPromoted()
        {
            int sellerid = (int)Session["SellerId"];
            var db = new AppDbContext();
            var products = db.Products.Where(x => x.SellerId == sellerid && x.ProductStatusId == 2 && x.Promote != null).Join(db.ProductStocks, x => x.ProductId, y => y.ProductId, (x, y) => new
            {
                x.ProductId,
                x.ProductImagePathOne,
                x.ProductName,
                x.Price,
                y.QuantitySold,
                x.SubcategoryId,
            });
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTop10Shipments()
        {
            int sellerid = (int)Session["SellerId"];
            var db = new AppDbContext();
            var top10Shipments = db.Shipments.Where(x => x.SellerId == sellerid && x.ShipmentStatusId == 1)
                            .Join(db.ShipmentStatusDetails, x => x.ShipmentStatusId, y => y.ShipmentStatusId, (x, y) => new
                            {
                                shipmentdate = x.ShipmentDate,
                                shipmentnumber = x.ShipmentNumber,
                                shipmentstatus = y.ShipmentStatus,
                            }).OrderByDescending(x => x.shipmentdate).Take(10).ToList().Select(x => new
                            {
                                shipmentdate = x.shipmentdate.ToString(),
                                shipmentnumber = x.shipmentnumber,
                                shipmentstatus = x.shipmentstatus,
                            });
            return Json(top10Shipments, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getChatMembers()
        {
            int sellerid = (int)Session["SellerId"];
            var db = new AppDbContext();
            var chatLists = db.ChatroomMembers.Where(x => x.SellerId == sellerid).Join(db.Users, x =>x.UserId,y =>y.UserId,(x,y) => new {
            UserId = x.UserId,
            UserName = y.UserName,
            ChatroomId = x.ChatroomId,
            }).ToList();
            return Json(chatLists, JsonRequestBehavior.AllowGet);
        }
    }
}