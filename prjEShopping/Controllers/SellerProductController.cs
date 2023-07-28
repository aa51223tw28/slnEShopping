using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{

    
    public class SellerProductController : Controller
    {
        List<string> optionIdsString = new List<string>();
        List<int> optionIds = new List<int>();
        
        // GET: SellerProduct
        public ActionResult SellerProductList()
        {
            var db = new AppDbContext();
            var products = db.Products.Where(x => x.SellerId == 1).Join(db.ProductStocks, x => x.ProductId, y => y.ProductId, (x, y) => new
            {
                ProductID = x.ProductId,
                ProductName = x.ProductName,
                Price = x.Price,
                ProductImagePathOne = x.ProductImagePathOne,
                StockQuantity = y.StockQuantity,
                OrderQuantity = y.OrderQuantity,
                ProductStatusId = x.ProductStatusId
            }).Select(x => new SellerProductListVM
            {
                ProductID = x.ProductID,
                ProductName = x.ProductName,
                Price = (int)x.Price,
                ProductImagePathOne = x.ProductImagePathOne,
                StockQuantity = x.StockQuantity,
                OrderQuantity = x.OrderQuantity,
                ProductStatusName = (db.ProductStatusDetails.Where(y => y.ProductStatusId == x.ProductStatusId).FirstOrDefault().ProductStatusName).ToString()
            }).ToList();

            return View(products);
        }
        public ActionResult ProductEdit(int? id)
        {
            var db = new AppDbContext();
            getAllOptionIds(id);
            int OptionIdOne = optionIds[0];
            int OptionIdTwo = optionIds[1];
            int OptionIdThree = optionIds[2];
            int OptionIdFour = optionIds[3];
            int OptionIdFive = optionIds[4];
            string CategoryName = db.ProductMainCategories.FirstOrDefault(y => y.CategoryId == (db.Products.FirstOrDefault(z => z.ProductId == id).CategoryId)).CategoryName;
            string SubcategoryName = db.ProductSubCategories.FirstOrDefault(y => y.SubcategoryId == (db.Products.FirstOrDefault(z => z.ProductId == id).SubcategoryId)).SubcategoryName;
            string SpecificationName0 = db.ProductSpecifications.FirstOrDefault(y => y.SpecificationId == (db.ProductOptions.FirstOrDefault(z => z.OptionId == OptionIdOne)).SpecificationId).SpecificationName;
            string SpecificationName1 = db.ProductSpecifications.FirstOrDefault(y => y.SpecificationId == (db.ProductOptions.FirstOrDefault(z => z.OptionId == OptionIdTwo)).SpecificationId).SpecificationName;
            string SpecificationName2 = db.ProductSpecifications.FirstOrDefault(y => y.SpecificationId == (db.ProductOptions.FirstOrDefault(z => z.OptionId == OptionIdThree)).SpecificationId).SpecificationName;
            string SpecificationName3 = db.ProductSpecifications.FirstOrDefault(y => y.SpecificationId == (db.ProductOptions.FirstOrDefault(z => z.OptionId == OptionIdFour)).SpecificationId).SpecificationName;
            string SpecificationName4 = db.ProductSpecifications.FirstOrDefault(y => y.SpecificationId == (db.ProductOptions.FirstOrDefault(z => z.OptionId == OptionIdFive)).SpecificationId).SpecificationName;
            string OptionName0 = db.ProductOptions.FirstOrDefault(y => y.OptionId == OptionIdOne).OptionName;
            string OptionName1 = db.ProductOptions.FirstOrDefault(y => y.OptionId == OptionIdTwo).OptionName;
            string OptionName2 = db.ProductOptions.FirstOrDefault(y => y.OptionId == OptionIdThree).OptionName;
            string OptionName3 = db.ProductOptions.FirstOrDefault(y => y.OptionId == OptionIdFour).OptionName;
            string OptionName4 = db.ProductOptions.FirstOrDefault(y => y.OptionId == OptionIdFive).OptionName;
            string BrandName = db.Brands.FirstOrDefault(y => y.BrandId == (db.Products.FirstOrDefault(G => G.ProductId == id).BrandId)).BrandName;
            var x = db.Products.FirstOrDefault(y => y.ProductId == id);
            var datashow = new SellerProductCreateVM
            {
                ProductID = x.ProductId,
                ProductName = x.ProductName,
                Price = x.Price,
                ProductDescription = x.ProductDescription,
                ProductImagePathOne = x.ProductImagePathOne,
                ProductImagePathTwo = x.ProductImagePathTwo,
                ProductImagePathThree = x.ProductImagePathThree,
                CategoryName = CategoryName,
                SubcategoryName = SubcategoryName,
                SpecificationName0 = SpecificationName0,
                SpecificationName1 = SpecificationName1,
                SpecificationName2 = SpecificationName2,
                SpecificationName3 = SpecificationName3,
                SpecificationName4 = SpecificationName4,
                OptionName0 = OptionName0,
                OptionName1 = OptionName1,
                OptionName2 = OptionName2,
                OptionName3 = OptionName3,
                OptionName4 = OptionName4,
                BrandName = BrandName,
            };
            return View(datashow);
        }
        [HttpPost]
        public ActionResult ProductEdit(SellerProductCreateVM vm) 
        {
            var db = new AppDbContext();
            var product = db.Products.FirstOrDefault(x => x.ProductId == vm.ProductID);

                product.ProductDescription = vm.ProductDescription;
                product.Price = vm.Price;
                product.ProductImagePathOne = vm.ProductImagePathOne;
                product.ProductImagePathTwo = vm.ProductImagePathTwo;
                product.ProductImagePathThree = vm.ProductImagePathThree;
                db.SaveChanges();
            if(vm.Quantity > 0 && vm.Quantity != null)
            { 
            var stock = db.ProductStocks.FirstOrDefault(x => x.ProductId == vm.ProductID);
                stock.StockQuantity = stock.StockQuantity + vm.Quantity;
                db.SaveChanges();

            var createstock = new ProductInventory()
            {
                ProductId = vm.ProductID,
                SellerId = 1,
                Quantity = vm.Quantity,
            };
            db.ProductInventories.Add(createstock);
                db.SaveChanges();
            }

            return RedirectToAction("SellerProductList");
        }
            public ActionResult ChangeStatus(int? id)
        {
            var db = new AppDbContext();
            var product = db.Products.Find(id);
            product.ProductStatusId = 1;
            db.SaveChanges();

            return RedirectToAction("SellerProductList");
        }


        public void getAllOptionIds(int? id)
        {
            var db = new AppDbContext();
            var optionIdsQuery = db.Products
                                  .Where(x => x.ProductId == id)
                                  .Select(x => new List<string>
                                  {
                                    x.OptionIdOne,
                                    x.OptionIdTwo,
                                    x.OptionIdThree,
                                    x.OptionIdFour,
                                    x.OptionIdFive
                                  })
                                  .FirstOrDefault();

            if (optionIdsQuery != null)
            {
                optionIdsString.AddRange(optionIdsQuery);
            }

            optionIds = optionIdsString
                .Select(x => int.Parse(x))
                .ToList();
 
        } 
    }
}