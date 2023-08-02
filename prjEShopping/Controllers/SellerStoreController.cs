using prjEShopping.Models.DTOs;
using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class SellerStoreController : Controller
    {
        // GET: SellerStore
        public ActionResult StoreMain()
        {
            var db = new AppDbContext();
            ViewBag.AllRatingStar = db.Products.Where(x => x.SellerId == 1).Join(db.Ratings, x => x.ProductId, y => y.ProductId, (x, y) => y.StarRating).Sum();
            ViewBag.RatingCount = db.Products.Where(x => x.SellerId == 1).Join(db.Ratings, x => x.ProductId, y => y.ProductId, (x, y) => y.RatingId).Count(); 
            ViewBag.AvgRating = ((double)(ViewBag.AllRatingStar)/ (ViewBag.RatingCount)).ToString("F1");
            ViewBag.Storename = db.Sellers.FirstOrDefault(x => x.SellerId == 1).SellerName;
            ViewBag.StoreIntro = db.Sellers.FirstOrDefault(x => x.SellerId == 1).StoreIntro;
            ViewBag.SellerId = 1;
            ViewBag.CollectedCount = 0;
            ViewBag.StoreImage = db.Sellers.FirstOrDefault(x => x.SellerId == 1).SellerImagePath;
            var products = db.Products.Where(x => x.SellerId == 1 && x.ProductStatusId == 2).Join(db.ProductStocks, x => x.ProductId, y => y.ProductId, (x, y) => new
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Price = x.Price,
                ProductImagePathOne = x.ProductImagePathOne,
                OrderQuantity = y.OrderQuantity,
                StockQuantity = y.StockQuantity,
                SubcategoryId = x.SubcategoryId,
                BrandId = x.BrandId,
            }).ToList()
              .Where(x => x.StockQuantity - x.OrderQuantity > 0)
              .Select(x => new Product{
                  ProductId = x.ProductId,
                  ProductName = x.ProductName,
                  Price = x.Price,
                  ProductImagePathOne = x.ProductImagePathOne,
                  SubcategoryId = x.SubcategoryId,
                  BrandId = x.BrandId,
              }).ToList();

            var CategoriesGroupDetails = products.Join(db.ProductSubCategories, x => x.SubcategoryId, y => y.SubcategoryId, (x, y) => new
            {
                ProductId = x.ProductId,
                CategoryId = y.CategoryId,
                SubcategoryId = y.SubcategoryId,
                SubcateName = y.SubcategoryName,
            })
                .Join(db.ProductMainCategories, x => x.CategoryId, y => y.CategoryId, (x, y) => new 
                {
                    SubcategoryId = x.SubcategoryId,
                    SubcateName = x.SubcateName,
                    CategoryId = y.CategoryId,
                    CategoryName = y.CategoryName,
                })
                .Select(x => new SellerStoreGroupByCategoryIdDto
                {
                    SubcategoryId = (int)x.SubcategoryId,
                    SubcateName = x.SubcateName,
                    CategoryId = (int)x.CategoryId,
                    CategoryName = x.CategoryName,
                })
                .ToList();
            ViewBag.CategoriesGroupDetails = CategoriesGroupDetails;
            ViewBag.SubcategoryIds = CategoriesGroupDetails.Select(x => x.SubcategoryId).Distinct().ToArray();

            var Brands = products.Select(x => new{ BrandId = x.BrandId } ).Distinct().Join(db.Brands, x => x.BrandId,y =>y.BrandId,(x,y)=> new  Brand
            {
                BrandId = y.BrandId,
                BrandName = y.BrandName,
            }).ToList();
            ViewBag.Brands = Brands;
            ViewBag.BrandsIds = Brands.Select(x => x.BrandId).ToArray();

            return View(products);
        }

        public ActionResult getTop5Product() 
        {
            var db = new AppDbContext();
            var products = db.Products.Where(x => x.SellerId == 1).Join(db.ProductStocks, x => x.ProductId, y => y.ProductId, (x, y) => new 
                        {
                            x.ProductId,
                            x.ProductImagePathOne,
                            x.ProductName,
                            PriceWithCurrency = "NT$" + ((int)x.Price).ToString(),
                            y.QuantitySold,
                            x.SubcategoryId,
                        } ).OrderByDescending(x => x.QuantitySold).Take(5);

            return Json(products, JsonRequestBehavior.AllowGet);
        }
    }
}