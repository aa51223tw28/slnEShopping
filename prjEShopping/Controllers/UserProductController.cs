using prjEShopping.Models.DTOs;
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
        public ActionResult ProdutList()
        {
            var db = new AppDbContext();
            List<Product> product = db.Products.ToList();
            return View(product);
        }

        public ActionResult SingleProduct(int productId)
        {            
            var db=new AppDbContext();
            var product = db.Products.Where(x => x.ProductId == productId).FirstOrDefault();
            return View(product);
        }
    }
}