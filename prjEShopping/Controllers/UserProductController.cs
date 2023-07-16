using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class UserProductController : Controller
    {
        // GET: UserProduct
        public ActionResult Index()
        {
            var db = new AppDbContext();
            List<Product> product = db.Products.ToList();
            return View(product);
        }

        public ActionResult SingleProduct()
        {            
            return View();
        }
    }
}