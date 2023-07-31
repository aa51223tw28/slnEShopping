using prjEShopping.Models.EFModels;
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
        public ActionResult Index(int id)
        {
            // 根據賣家的 ID 從資料庫中讀取賣家資料
            var db = new AppDbContext();
            var seller = db.Sellers.FirstOrDefault(x => x.SellerId == id);

            // 將賣家資料傳遞給 SellerMain 頁面
            return View(seller);
        }
    }
}