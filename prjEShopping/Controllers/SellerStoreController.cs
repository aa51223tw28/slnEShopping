using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class SellerStoreController : Controller
    {
        // GET: SellerStore
        public ActionResult StoreMain()
        {
            var db = new AppDbContext();
            ViewBag.AllRatingStar = 29;
            ViewBag.RatingCount = 7;
            ViewBag.AvgRating = ((double)(ViewBag.AllRatingStar)/ (ViewBag.RatingCount)).ToString("F1");
            ViewBag.Storename = db.Sellers.FirstOrDefault(x => x.SellerId == 1).SellerName;
            ViewBag.StoreIntro = db.Sellers.FirstOrDefault(x => x.SellerId == 1).StoreIntro;
            ViewBag.CollectedCount = 0;
            ViewBag.StoreImage = db.Sellers.FirstOrDefault(x => x.SellerId == 1).SellerImagePath;
            var products = db.Products.Where(x => x.SellerId == 1).ToList();
            return View(products);
        }
    }
}