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
        public ActionResult Index()
        {
            int id = (int)Session["SellerId"];
            // 根據賣家的 ID 從資料庫中讀取賣家資料
            var db = new AppDbContext();
            var seller = db.Sellers.FirstOrDefault(x => x.SellerId == id);

            // 將賣家資料傳遞給 SellerMain 頁面
            return View(seller);
        }

        public ActionResult SaveOrClearFavorite(int sellerid,int userid) 
        {
            var db = new AppDbContext();
            var isInFavorite = db.TrackSellers.FirstOrDefault(x => x.SellerId == sellerid && x.UserId == userid);
            if (isInFavorite != null)
            {
                db.TrackSellers.Remove(isInFavorite);
            }
            else 
            {
                var addtofavorite = new TrackSeller
                {
                    UserId = userid,
                    SellerId = sellerid,
                };
                db.TrackSellers.Add(addtofavorite);
            }
            db.SaveChanges();
            return Json(1,JsonRequestBehavior.AllowGet);
        }
    }
}