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
    }
}