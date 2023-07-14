using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class SellerUpdateController : Controller
    {
        // GET: SellerUpdate
        public ActionResult List()
        {
            var db = new AppDbContext();
            Seller sellers = new Seller();
            sellers = db.Sellers.Where(x =>x.SellerId ==1).SingleOrDefault();


            return View(sellers);
        }
    }
}