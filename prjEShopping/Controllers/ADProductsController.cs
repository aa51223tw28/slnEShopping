using Newtonsoft.Json;
using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class ADProductsController : Controller
    {
        private AppDbContext db = new AppDbContext();
        // GET: ADProducts
        public ActionResult Index()
        {
            HttpCookie authCookie = Request.Cookies["AdminLogin"];
            if (authCookie == null || authCookie.Values["status"] != "AdminLogin" || authCookie.Values["AccessRightId"] != "1")
            {
                return RedirectToAction("Login", "Admins");
            }
            string decodedName = HttpUtility.UrlDecode(authCookie.Values["userName"]);
            ViewBag.AdminName = decodedName;

            var sellersAD = db.SellersADs.Select(x => x.ADProductId).ToList();
            ViewBag.SellersAD = sellersAD;
            var model = db.ADProducts.ADProduct2VM();
            return View(model);
        }

         public ActionResult CreateAD()
        {
            HttpCookie authCookie = Request.Cookies["AdminLogin"];
            if (authCookie == null || authCookie.Values["status"] != "AdminLogin" || authCookie.Values["AccessRightId"] != "1")
            {
                return RedirectToAction("Login", "Admins");
            }
            string decodedName = HttpUtility.UrlDecode(authCookie.Values["userName"]);
            ViewBag.AdminName = decodedName;

            var ad=new ADProduct();
            ADProductVM model = ADProductChange.ADProduct2VM(ad);
            
            //var data = db.ADProducts.ToList(); // 取得資料庫內容
           
            //ViewBag.JsonData = data;

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateAD(ADProductVM vm)
        {
            if(vm == null)
                return View(vm);

            var ad = ADProductChange.VM2ADProduct(vm);

            db.ADProducts.Add(ad);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}