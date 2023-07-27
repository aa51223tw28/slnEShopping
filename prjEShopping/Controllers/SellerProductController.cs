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

        List<string> optionids = new List<string>();
        List<string> optionames = new List<string>();
        List<string> specification = new List<string>();
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
            

            getAllOptionIds(id);
            getAllOptionNames();
            getAllSpecificationNames();

            var db = new AppDbContext();

             var datashow = db.Products.Where(x => x.ProductId == id).Select(x => new SellerProductCreateVM
            {
                ProductID = x.ProductId,
                ProductName = x.ProductName,
                Price = x.Price,
                ProductDescription = x.ProductDescription,
                BrandName = db.Brands.FirstOrDefault(y => y.BrandId == x.BrandId).BrandName,
                ProductImagePathOne = x.ProductImagePathOne,
                ProductImagePathTwo = x.ProductImagePathTwo,
                ProductImagePathThree = x.ProductImagePathThree,
                CategoryName = db.ProductMainCategories.FirstOrDefault(y => y.CategoryId == x.CategoryId).CategoryName,
                SubcategoryName = db.ProductSubCategories.FirstOrDefault(y => y.SubcategoryId == x.SubcategoryId).SubcategoryName,
                SpecificationName0 = specification[0],
                SpecificationName1 = specification[1],
                SpecificationName2 = specification[2],
                SpecificationName3 = specification[3],
                SpecificationName4 = specification[4],
                OptionName0 = optionames[0],
                OptionName1 = optionames[1],
                OptionName2 = optionames[2],
                OptionName3 = optionames[3],
                OptionName4 = optionames[4],
            }); 


            return View(datashow);
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
            var optionIds = db.Products
                              .Where(x => x.ProductId == id)
                              .Select(x => new
                              {
                                  OptionIdOne = x.OptionIdOne,
                                  OptionIdTwo = x.OptionIdTwo,
                                  OptionIdThree = x.OptionIdThree,
                                  OptionIdFour = x.OptionIdFour,
                                  OptionIdFive = x.OptionIdFive
                              })
                              .ToList();

            foreach (var optionId in optionIds)
            {
                optionids.Add(optionId.OptionIdOne);
                optionids.Add(optionId.OptionIdTwo);
                optionids.Add(optionId.OptionIdThree);
                optionids.Add(optionId.OptionIdFour);
                optionids.Add(optionId.OptionIdFive);
            }
        }

        public void getAllOptionNames() 
        {
            var db = new AppDbContext();
            
            foreach (var idnames in optionids) 
            {
                int intid;
                intid = Int32.Parse(idnames);
                var datas = db.ProductOptions.SingleOrDefault(x => x.OptionId == intid).OptionName;
                optionames.Add(datas);
            }
         
        }

        public void getAllSpecificationNames() 
        {
            var db = new AppDbContext();
            foreach (var optionid in optionids) 
            {
                int intid;
                intid = Int32.Parse(optionid);
                var datas = db.ProductOptions.Where(x => x.OptionId == intid)
                    .Join(db.ProductSpecifications, x => x.SpecificationId, y => y.SpecificationId, (x, y) =>

                     y.SpecificationName

                ).FirstOrDefault();
                specification.Add(datas);
            }

        }
    }
}