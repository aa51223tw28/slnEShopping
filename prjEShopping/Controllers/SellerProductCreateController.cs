﻿using prjEShopping.Models.EFModels;
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
                                
                                    );
            return Json(subcategorynames, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getOptionNames(string subcategoryname) 
        {
            AppDbContext db = new AppDbContext();
            var optionnames = db.ProductSubCategories.Where(x => x.SubcategoryName == subcategoryname)
                           .Join(db.ProductSpecifications, x => x.SubcategoryId, y => y.SubcategoryId, (x, y) =>

                           y.SpecificationName

                           );
            return Json(optionnames, JsonRequestBehavior.AllowGet);
        }

    }
}