﻿using prjEShopping.Models.DTOs;
using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            ViewBag.StoreImage = db.Sellers.FirstOrDefault(x => x.SellerId == 1).SellerImagePath;
            ViewBag.TrackSeller = db.TrackSellers.Any(x => x.SellerId == 1 && x.UserId == 1);
            ViewBag.TrackCount = db.TrackSellers.Where(x => x.SellerId == 1).Count();

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
            var products = db.Products.Where(x => x.SellerId == 1 && x.ProductStatusId == 2).Join(db.ProductStocks, x => x.ProductId, y => y.ProductId, (x, y) => new 
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

        public ActionResult getPromoted() 
        {
            var db = new AppDbContext();
            var products = db.Products.Where(x => x.SellerId == 1 && x.ProductStatusId == 2 && x.Promote != null).Join(db.ProductStocks, x => x.ProductId, y => y.ProductId, (x, y) => new
            {
                x.ProductId,
                x.ProductImagePathOne,
                x.ProductName,
                PriceWithCurrency = "NT$" + ((int)x.Price).ToString(),
                y.QuantitySold,
                x.SubcategoryId,
            });
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveOrClearFavorite(int sellerid, int userid)
        {
            var db = new AppDbContext();
            var isInFavorite = db.TrackSellers.FirstOrDefault(x => x.SellerId == sellerid && x.UserId == userid);
            if (isInFavorite != null)
            {
                db.TrackSellers.Remove(isInFavorite);
            }
            else
            {
                var addtofavorite = new TrackSeller
                {
                    UserId = userid,
                    SellerId = sellerid,
                };
                db.TrackSellers.Add(addtofavorite);
            }
            db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowAllStore() 
        {
            var db = new AppDbContext();
            var products = db.Sellers.GroupJoin(db.TrackSellers, x => x.SellerId, y => y.SellerId, (x, y) => new UserAllStoreListVM
            {
                SellerId = x.SellerId,
                StoreName = x.StoreName,
                SellerImagePath = x.SellerImagePath,
                TrackCount = y.Count(),
            }).ToList();

            return View(products);
        }

        public ActionResult ShowAllStore2()
        {
            var db = new AppDbContext();
            var products = db.Sellers.GroupJoin(db.TrackSellers, x => x.SellerId, y => y.SellerId, (x, y) => new UserAllStoreListVM
            {
                SellerId = x.SellerId,
                StoreName = x.StoreName,
                SellerImagePath = x.SellerImagePath,
                TrackCount = y.Count(),
            }).ToList();

            return Json(products, JsonRequestBehavior.AllowGet);
        }
    }
}