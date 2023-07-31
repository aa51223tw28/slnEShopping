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
            ViewBag.AllRatingStar = db.Products.Where(x => x.SellerId == 1).Join(db.Ratings, x => x.ProductId, y => y.ProductId, (x, y) => y.StarRating).Sum();
            ViewBag.RatingCount = db.Products.Where(x => x.SellerId == 1).Join(db.Ratings, x => x.ProductId, y => y.ProductId, (x, y) => y.RatingId).Count(); 
            ViewBag.AvgRating = ((double)(ViewBag.AllRatingStar)/ (ViewBag.RatingCount)).ToString("F1");
            ViewBag.Storename = db.Sellers.FirstOrDefault(x => x.SellerId == 1).SellerName;
            ViewBag.StoreIntro = db.Sellers.FirstOrDefault(x => x.SellerId == 1).StoreIntro;
            ViewBag.CollectedCount = 0;
            ViewBag.StoreImage = db.Sellers.FirstOrDefault(x => x.SellerId == 1).SellerImagePath;
            var products = db.Products.Where(x => x.SellerId == 1).ToList();
            return View(products);
        }

        public ActionResult getTop5Product() 
        {
            var db = new AppDbContext();
            var products = db.Products.Where(x => x.SellerId == 1).Join(db.ProductStocks, x => x.ProductId, y => y.ProductId, (x, y) => new 
                        {
                            x.ProductId,
                            x.ProductImagePathOne,
                            x.ProductName,
                            PriceWithCurrency = "NT$" + ((int)x.Price).ToString(),
                            y.QuantitySold,
                        } ).OrderByDescending(x => x.QuantitySold).Take(5);
            return Json(products, JsonRequestBehavior.AllowGet);
        }
    }
}