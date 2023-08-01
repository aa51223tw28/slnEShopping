using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var model = db.ADProducts.ADProduct2VM();
            return View(model);
        }

         public ActionResult CreateAD()
        { 
            var ad=new ADProduct();
            ADProductVM model = ADProductChange.ADProduct2VM(ad);
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