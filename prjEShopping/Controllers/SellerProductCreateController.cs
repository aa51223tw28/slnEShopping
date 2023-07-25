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

        [HttpPost]
        public ActionResult ProductCreate(SellerProductCreateVM vm)
        {
            var db = new AppDbContext();
            var product = new Product()
            {
                ProductName = vm.ProductName,
                ProductDescription = vm.ProductDescription,
                Price = vm.Price,
                BrandId = (db.Brands.Where(x => x.BrandName == vm.BrandName).SingleOrDefault()).BrandId,
                ProductStatusId = 2,
                SellerId = 1,
            };
            db.Products.Add(product);
            db.SaveChanges();

            var productdetail = new ProductDetail()
            {
                ProductId = 25,
                CategoryId = (db.ProductMainCategories.Where(x => x.CategoryName == vm.CategoryName).SingleOrDefault()).CategoryId,
                SubcategoryId = (db.ProductSubCategories.Where(x => x.SubcategoryName == vm.SubcategoryName).SingleOrDefault()).SubcategoryId,
                OptionIdOne = (db.ProductOptions.Where(x => x.OptionName == vm.OptionName0 && x.SpecificationId == (db.ProductSpecifications.Where(y => y.SpecificationName == vm.SpecificationName0 && y.SubcategoryId == (db.ProductSubCategories.Where(z => z.SubcategoryName == vm.SubcategoryName).FirstOrDefault().SubcategoryId)).FirstOrDefault()).SpecificationId).SingleOrDefault().OptionId).ToString(),
                OptionIdTwo = (db.ProductOptions.Where(x => x.OptionName == vm.OptionName1 && x.SpecificationId == (db.ProductSpecifications.Where(y => y.SpecificationName == vm.SpecificationName1 && y.SubcategoryId == (db.ProductSubCategories.Where(z => z.SubcategoryName == vm.SubcategoryName).FirstOrDefault().SubcategoryId)).FirstOrDefault()).SpecificationId).SingleOrDefault().OptionId).ToString(),
                OptionIdThree = (db.ProductOptions.Where(x => x.OptionName == vm.OptionName2 && x.SpecificationId == (db.ProductSpecifications.Where(y => y.SpecificationName == vm.SpecificationName2 && y.SubcategoryId == (db.ProductSubCategories.Where(z => z.SubcategoryName == vm.SubcategoryName).FirstOrDefault().SubcategoryId)).FirstOrDefault()).SpecificationId).SingleOrDefault().OptionId).ToString(),
                OptionIdFour = (db.ProductOptions.Where(x => x.OptionName == vm.OptionName3 && x.SpecificationId == (db.ProductSpecifications.Where(y => y.SpecificationName == vm.SpecificationName3 && y.SubcategoryId == (db.ProductSubCategories.Where(z => z.SubcategoryName == vm.SubcategoryName).FirstOrDefault().SubcategoryId)).FirstOrDefault()).SpecificationId).SingleOrDefault().OptionId).ToString(),
                OptionIdFive = (db.ProductOptions.Where(x => x.OptionName == vm.OptionName4 && x.SpecificationId == (db.ProductSpecifications.Where(y => y.SpecificationName == vm.SpecificationName4 && y.SubcategoryId == (db.ProductSubCategories.Where(z => z.SubcategoryName == vm.SubcategoryName).FirstOrDefault().SubcategoryId)).FirstOrDefault()).SpecificationId).SingleOrDefault().OptionId).ToString(),
            };
            db.ProductDetails.Add(productdetail);
            db.SaveChanges();

            return RedirectToAction("Index", "SellerMain");
        }
    }
}