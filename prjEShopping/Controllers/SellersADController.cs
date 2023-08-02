using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class SellersADController : Controller
    {
        private AppDbContext db = new AppDbContext();
        // GET: SellersAD
        public ActionResult List()
        {
            var model = db.ADProducts.ADProduct2VM();
            return View(model);
        }
    }
}