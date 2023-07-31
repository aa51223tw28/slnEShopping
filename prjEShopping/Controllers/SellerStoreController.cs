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
            var products = db.Products.Where(x => x.SellerId == 1).ToList();
            return View(products);
        }
    }
}