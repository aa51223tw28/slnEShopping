using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class SellerProductCreateController : Controller
    {
        // GET: SellerProductCreate
        public ActionResult getCategoryName()
        {
            AppDbContext db = new AppDbContext();
            var categorynames = db.ProductMainCategories.Select(x => x.CategoryName).Distinct();
            return Json(categorynames,JsonRequestBehavior.AllowGet);
        }

        public ActionResult getSubcategoryName(string categoryname) 
        {
            AppDbContext db = new AppDbContext();
            var subcategorynames = db.ProductMainCategories.Where(x => x.CategoryName == categoryname)
                                .Join(db.ProductSubCategories, x => x.CategoryId, y => y.CategoryId, (x, y) => 
                                
                                    y.SubcategoryName
                                
                                    ).ToList();
            return Json(subcategorynames, JsonRequestBehavior.AllowGet);
        }
    }
}