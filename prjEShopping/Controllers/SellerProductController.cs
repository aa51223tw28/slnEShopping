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
            int sellerid = (int)Session["SellerId"];
            var db = new AppDbContext();
            var products = db.Products.Where(x => x.SellerId == sellerid).Join(db.ProductStocks, x => x.ProductId, y => y.ProductId, (x, y) => new
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
        public ActionResult ProductEdit(SellerProductCreateVM vm, HttpPostedFileBase photo1, HttpPostedFileBase photo2, HttpPostedFileBase photo3) 
        {
            int sellerid = (int)Session["SellerId"];
            var db = new AppDbContext();
            var product = db.Products.FirstOrDefault(x => x.ProductId == vm.ProductID);

            if (vm.photo1 != null)
            {
                string filename1 = Guid.NewGuid().ToString() + ".jpg";
                string imagePath1 = Server.MapPath("~/img/" + filename1);
                vm.photo1.SaveAs(imagePath1);
                product.ProductImagePathOne = filename1;
            }

            if (vm.photo2 != null)
            {
                string filename2 = Guid.NewGuid().ToString() + ".jpg";
                string imagePath2 = Server.MapPath("~/img/" + filename2);
                vm.photo2.SaveAs(imagePath2);
                product.ProductImagePathTwo = filename2;
            }

            if (vm.photo3 != null)
            {
                string filename3 = Guid.NewGuid().ToString() + ".jpg";
                string imagePath3 = Server.MapPath("~/img/" + filename3);
                vm.photo3.SaveAs(imagePath3);
                product.ProductImagePathThree = filename3;
            }
                product.ProductDescription = vm.ProductDescription;
                product.Price = vm.Price;
                
                db.SaveChanges();
            if(vm.Quantity > 0 && vm.Quantity != null)
            { 
            var stock = db.ProductStocks.FirstOrDefault(x => x.ProductId == vm.ProductID);
                stock.StockQuantity = stock.StockQuantity + vm.Quantity;
                db.SaveChanges();

            var createstock = new ProductInventory()
            {
                ProductId = vm.ProductID,
                SellerId = sellerid,
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