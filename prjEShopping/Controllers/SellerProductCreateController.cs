using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class SellerProductCreateController : Controller
    {
        // GET: SellerProduct
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult ProductCreate()
		{
			return View();
		}

        //[HttpPost]
        //public ActionResult ProductCreate(SellerProductCreateVM vm)
        //{
        //    var db = new AppDbContext();
        //    var product = new Product()
        //    {
        //        ProductName = vm.ProductName,
        //        ProductDescription = vm.ProductDescription,
        //        Price = vm.Price,
        //        BrandId = (db.Brands.Where(x => x.BrandName == vm.BrandName).SingleOrDefault()).BrandId,
        //        ProductStatusId = 2,
        //    };
        //    db.Products.Add(product);
        //    db.SaveChanges();

        //    var productdetail = new ProductDetail()
        //    {
        //        CategoryId = (db.ProductMainCategories.Where(x => x.CategoryName == vm.CategoryName).SingleOrDefault()).CategoryId,
        //        SubcategoryId = (db.ProductSubCategories.Where(x => x.SubcategoryName == vm.SubcategoryName).SingleOrDefault()).SubcategoryId,
        //        OptionIdOne = db.ProductOptions.Where(x => x.SpecificationId == (db.ProductSpecifications.Where(y =>y.SpecificationName == vm.SpecificationName1).SingleOrDefault().SpecificationId).SingleOrDefault()).OptionId,
        //    };
        //    return RedirectToAction("Index","SellerMain");
        //}
    }
}