using prjEShopping.Models.DTOs;
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
        public ActionResult ProductCreate(SellerProductCreateVM vm , HttpPostedFileBase photo1, HttpPostedFileBase photo2 , HttpPostedFileBase photo3)
        {
            int sellerid = (int)Session["SellerId"];
            var db = new AppDbContext();
            var product = new Product();
            
            if (vm.photo1 != null)
            { 
                string filename1 = Guid.NewGuid().ToString() + ".jpg";
                string imagePath = Server.MapPath("~/img/" + filename1);
                vm.photo1.SaveAs(imagePath);
                product.ProductImagePathOne = filename1;
            }

            if (vm.photo2 != null)
            {
                string filename2 = Guid.NewGuid().ToString() + ".jpg";
                string imagePath = Server.MapPath("~/img/" + filename2);
                vm.photo1.SaveAs(imagePath);
                product.ProductImagePathTwo = filename2;
            }

            if (vm.photo3 != null)
            {
                string filename3 = Guid.NewGuid().ToString() + ".jpg";
                string imagePath = Server.MapPath("~/img/" + filename3);
                vm.photo1.SaveAs(imagePath);
                product.ProductImagePathThree = filename3;
            }

                product.ProductName = vm.ProductName;
                product.ProductDescription = vm.ProductDescription;
                product.Price = vm.Price;
                product.BrandId = (db.Brands.Where(x => x.BrandName == vm.BrandName).SingleOrDefault()).BrandId;
                product.ProductStatusId = 2;
                product.SellerId = sellerid;
                product.CategoryId = (db.ProductMainCategories.Where(x => x.CategoryName == vm.CategoryName).SingleOrDefault()).CategoryId;
                product.SubcategoryId = (db.ProductSubCategories.Where(x => x.SubcategoryName == vm.SubcategoryName).SingleOrDefault()).SubcategoryId;
                product.OptionIdOne = (db.ProductOptions.Where(x => x.OptionName == vm.OptionName0 && x.SpecificationId == (db.ProductSpecifications.Where(y => y.SpecificationName == vm.SpecificationName0 && y.SubcategoryId == (db.ProductSubCategories.Where(z => z.SubcategoryName == vm.SubcategoryName).FirstOrDefault().SubcategoryId)).FirstOrDefault()).SpecificationId).SingleOrDefault().OptionId).ToString();
                product.OptionIdTwo = (db.ProductOptions.Where(x => x.OptionName == vm.OptionName1 && x.SpecificationId == (db.ProductSpecifications.Where(y => y.SpecificationName == vm.SpecificationName1 && y.SubcategoryId == (db.ProductSubCategories.Where(z => z.SubcategoryName == vm.SubcategoryName).FirstOrDefault().SubcategoryId)).FirstOrDefault()).SpecificationId).SingleOrDefault().OptionId).ToString();
                product.OptionIdThree = (db.ProductOptions.Where(x => x.OptionName == vm.OptionName2 && x.SpecificationId == (db.ProductSpecifications.Where(y => y.SpecificationName == vm.SpecificationName2 && y.SubcategoryId == (db.ProductSubCategories.Where(z => z.SubcategoryName == vm.SubcategoryName).FirstOrDefault().SubcategoryId)).FirstOrDefault()).SpecificationId).SingleOrDefault().OptionId).ToString();
                product.OptionIdFour = (db.ProductOptions.Where(x => x.OptionName == vm.OptionName3 && x.SpecificationId == (db.ProductSpecifications.Where(y => y.SpecificationName == vm.SpecificationName3 && y.SubcategoryId == (db.ProductSubCategories.Where(z => z.SubcategoryName == vm.SubcategoryName).FirstOrDefault().SubcategoryId)).FirstOrDefault()).SpecificationId).SingleOrDefault().OptionId).ToString();
                product.OptionIdFive = (db.ProductOptions.Where(x => x.OptionName == vm.OptionName4 && x.SpecificationId == (db.ProductSpecifications.Where(y => y.SpecificationName == vm.SpecificationName4 && y.SubcategoryId == (db.ProductSubCategories.Where(z => z.SubcategoryName == vm.SubcategoryName).FirstOrDefault().SubcategoryId)).FirstOrDefault()).SpecificationId).SingleOrDefault().OptionId).ToString();
            
                db.Products.Add(product);
                db.SaveChanges();

            if (vm.Quantity > 0 && vm.Quantity != null)
            {
                int productid = db.Products.FirstOrDefault(x => x.ProductName == vm.ProductName && x.SellerId == sellerid).ProductId;

                var stock = new ProductStock()
                {
                    ProductId = productid,
                    PurchaseQuantity = 0,
                    OrderQuantity = 0,
                    QuantitySold = 0,
                    StockQuantity = vm.Quantity,
                };
                db.ProductStocks.Add(stock);
                db.SaveChanges();

                var createstock = new ProductInventory()
                {
                    ProductId = productid,
                    SellerId = sellerid,
                    Quantity = vm.Quantity,
                };
                db.ProductInventories.Add(createstock);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "SellerMain");
        }
    }
}
