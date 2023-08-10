﻿using prjEShopping.Models.DTOs;
using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class UserProductController : Controller
    {
        // GET: UserProduct
        public ActionResult UserProdutList()
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var cartid = db.ShoppingCarts.Where(x => x.UserId == userid).OrderByDescending(x => x.CartId).Select(x => x.CartId).FirstOrDefault();

            List<UserProductIndexDto> datas = new List<UserProductIndexDto>();

            var products = db.Products.ToList();

            foreach (var item in products)
            {
                var orderQuantity = db.ProductStocks.Where(x => x.ProductId == item.ProductId).Select(x => x.OrderQuantity).FirstOrDefault() ?? 0;

                var stockQuantity = db.ProductStocks.Where(x => x.ProductId == item.ProductId).Select(x => x.StockQuantity).FirstOrDefault() ?? 0;

                //商品狀態 1審核中/2販售中/3下架中
                var productstatus = products.Where(x => x.ProductId == item.ProductId).Select(x => x.ProductStatusId).FirstOrDefault();
                

                if (stockQuantity - orderQuantity > 0&& productstatus==2)
                {
                    var data = new UserProductIndexDto()
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Price = (decimal)item.Price,
                        ProductImagePathOne = item.ProductImagePathOne,
                        ProductStock = stockQuantity - orderQuantity,
                        QuantitySold= (int)db.ProductStocks.FirstOrDefault(x => x.ProductId == item.ProductId).QuantitySold,
                        SubcategoryId= (int)products.FirstOrDefault(x=>x.ProductId== item.ProductId).SubcategoryId,
                        BrandId=(int)item.BrandId,
                        //BrandName=db.Brands.FirstOrDefault(x=>x.BrandId== item.BrandId).BrandName,
                    };

                    datas.Add(data);
                }
            }

            ViewBag.Brand = db.Brands.ToList();

            return View(datas);
        }

        public ActionResult UserSingleProduct(int productId)
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var cartid = db.ShoppingCarts.Where(x => x.UserId == userid).OrderByDescending(x => x.CartId).Select(x => x.CartId).FirstOrDefault();

            var product = db.Products.FirstOrDefault(x => x.ProductId == productId);

            //剩餘多少數量計算式為ProductStocks table=StockQuantity-OrderQuantity
            var orderQuantity = db.ProductStocks.Where(x => x.ProductId == productId).Select(x=>x.OrderQuantity).FirstOrDefault() ?? 0;
            var stockQuantity= db.ProductStocks.Where(x => x.ProductId == productId).Select(x => x.StockQuantity).FirstOrDefault() ?? 0;

            //售出數量ProductStocks中的QuantitySold
            var quantitySold = db.ProductStocks.Where(x => x.ProductId == productId).Select(x => x.QuantitySold).FirstOrDefault();

            //一堆規格
            //品牌
            var brandId = db.Products.Where(x => x.ProductId == productId).Select(x => x.BrandId).FirstOrDefault();
            var brandName = db.Brands.Where(x => x.BrandId == brandId).Select(x => x.BrandName).FirstOrDefault();                     

            //選項內容
            var optionIdOne = db.Products.Where(x => x.ProductId == productId).Select(x => x.OptionIdOne).FirstOrDefault();
            var optionIdTwo = db.Products.Where(x => x.ProductId == productId).Select(x => x.OptionIdTwo).FirstOrDefault();
            var optionIdThree = db.Products.Where(x => x.ProductId == productId).Select(x => x.OptionIdThree).FirstOrDefault();
            var optionIdFour = db.Products.Where(x => x.ProductId == productId).Select(x => x.OptionIdFour).FirstOrDefault();
            var optionIdFive = db.Products.Where(x => x.ProductId == productId).Select(x => x.OptionIdFive).FirstOrDefault();

            int parsedOptionIdOne = int.Parse(optionIdOne);
            int parsedOptionIdTwo = int.Parse(optionIdTwo);
            int parsedOptionIdThree = int.Parse(optionIdThree);
            int parsedOptionIdFour = int.Parse(optionIdFour);
            int parsedOptionIdFive = int.Parse(optionIdFive);


            var optionNameOne = db.ProductOptions.Where(x => x.OptionId == parsedOptionIdOne).Select(x => x.OptionName).FirstOrDefault();
            var optionNameTwo = db.ProductOptions.Where(x => x.OptionId == parsedOptionIdTwo) .Select(x => x.OptionName).FirstOrDefault();
            var optionNameThree = db.ProductOptions.Where(x => x.OptionId == parsedOptionIdThree).Select(x => x.OptionName).FirstOrDefault();
            var optionNameFour = db.ProductOptions.Where(x => x.OptionId == parsedOptionIdFour).Select(x => x.OptionName).FirstOrDefault();
            var optionNameFive = db.ProductOptions.Where(x => x.OptionId == parsedOptionIdFive).Select(x => x.OptionName).FirstOrDefault();

            //選項名稱
            var SpecificationIdOne = db.ProductOptions.Where(x => x.OptionName == optionNameOne).Select(x => x.SpecificationId).FirstOrDefault();
            var SpecificationIdTwo = db.ProductOptions.Where(x => x.OptionName == optionNameTwo).Select(x => x.SpecificationId).FirstOrDefault();
            var SpecificationIdThree = db.ProductOptions.Where(x => x.OptionName == optionNameThree).Select(x => x.SpecificationId).FirstOrDefault();
            var SpecificationIdFour = db.ProductOptions.Where(x => x.OptionName == optionNameFour).Select(x => x.SpecificationId).FirstOrDefault();
            var SpecificationIdOFive = db.ProductOptions.Where(x => x.OptionName == optionNameFive).Select(x => x.SpecificationId).FirstOrDefault();

            var SpecificationNameOne=db.ProductSpecifications.Where(x=>x.SpecificationId== SpecificationIdOne).Select(x => x.SpecificationName).FirstOrDefault();
            var SpecificationNameTwo = db.ProductSpecifications.Where(x => x.SpecificationId == SpecificationIdTwo).Select(x => x.SpecificationName).FirstOrDefault();
            var SpecificationNameThree = db.ProductSpecifications.Where(x => x.SpecificationId == SpecificationIdThree).Select(x => x.SpecificationName).FirstOrDefault();
            var SpecificationNameFour = db.ProductSpecifications.Where(x => x.SpecificationId == SpecificationIdFour).Select(x => x.SpecificationName).FirstOrDefault();
            var SpecificationNameFive = db.ProductSpecifications.Where(x => x.SpecificationId == SpecificationIdOFive).Select(x => x.SpecificationName).FirstOrDefault();

            //大分類名稱
            var categoryId = db.Products.Where(x => x.ProductId == productId).Select(x => x.CategoryId).FirstOrDefault();
            var categoryName=db.ProductMainCategories.Where(x=>x.CategoryId== categoryId).Select(x => x.CategoryName).FirstOrDefault();

            //小分類名稱
            var subcategoryId=db.Products.Where(x=>x.ProductId== productId).Select(x=>x.SubcategoryId).FirstOrDefault();
            var subcategoryName=db.ProductSubCategories.Where(x=>x.SubcategoryId== subcategoryId).Select(x=>x.SubcategoryName).FirstOrDefault();

            var datas = new UserProductIndexDto()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Price = (decimal)product.Price,
                ProductImagePathOne = product.ProductImagePathOne,
                ProductImagePathTwo = product.ProductImagePathTwo,
                ProductImagePathThree = product.ProductImagePathThree,
                ProductStock= stockQuantity- orderQuantity,
                QuantitySold= (int)quantitySold,

                //一堆規格
                BrandName= brandName,
                SpecificationNameOne = SpecificationNameOne,
                SpecificationNameTwo = SpecificationNameTwo,
                SpecificationNameThree = SpecificationNameThree,
                SpecificationNameFour = SpecificationNameFour,
                SpecificationNameFive = SpecificationNameFive,

                OptionNameOne = optionNameOne,
                OptionNameTwo= optionNameTwo,
                OptionNameThree= optionNameThree,
                OptionNameFour= optionNameFour,
                OptionNameFive= optionNameFive,

                CategoryName= categoryName,
                SubcategoryName = subcategoryName,
            };
            //現在購物車該使用者該購物車id該商品的數量
            var qua = db.ShoppingCartDetails.Where(x => x.CartId == cartid && x.ProductId == productId).Select(x => x.Quantity).FirstOrDefault();
            if(qua != null)
            {
                ViewBag.TotalQuantity = qua;
            }
            else
            {
                ViewBag.TotalQuantity = 0;
            }            
            return View(datas);
        }



        [Authorize]
        public ActionResult TrackProductapi(int productId)//在商品頁面追蹤什麼商品
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            
            var trackproductId=db.TrackProducts.FirstOrDefault(x=>x.ProductId == productId && x.UserId == userid);

            if (trackproductId == null)
            {
                var data = new TrackProduct()
                {
                    UserId = userid,
                    ProductId = productId,
                };
                db.TrackProducts.Add(data);
                db.SaveChanges();
                return Content("TrackProduct");
            }
            else
            {
                var trackProductIdToDelete = trackproductId.TrackProductId;
                var trackProductToDelete = db.TrackProducts.FirstOrDefault(x => x.TrackProductId == trackProductIdToDelete&&x.ProductId== productId);
                if (trackProductToDelete != null)
                {
                    db.TrackProducts.Remove(trackProductToDelete);
                    db.SaveChanges();
                }
                return Content("noTrackProduct");
            }
        }


        [Authorize]
        public ActionResult starTrackProductapi(int productId)//一進入頁面判斷有無追蹤的api
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();

            var trackproductId = db.TrackProducts.FirstOrDefault(x => x.ProductId == productId&&x.UserId== userid);

            if (trackproductId == null)
            {               
                return Content("noTrackProduct");
            }
            else
            {                
                return Content("TrackProduct");
            }
        }


        //--------------Comment-----------------
    //    public ActionResult Comment()
    //    {
    //        using (var db = new AppDbContext()) // 假設您的 DbContext 名稱為 AppDbContext
    //        {
    //            // 從資料庫中查詢評分資料，假設評分資料存在 Ratings 表中
    //            List<Rating> ratingsList = db.Ratings.ToList();

    //            // 取得所有評分對應的回覆資料
    //            var ratingIds = ratingsList.Select(r => r.RatingId);
    //            var sellerReplies = db.RatingReplaies
    //                .Where(r => ratingIds.Contains(r.RatingId.Value))
    //                .GroupBy(r => r.RatingId.Value) // 按照 RatingId 分組
    //                .ToDictionary(g => g.Key, g => g.FirstOrDefault()?.ReplayText ?? string.Empty);
    //            ViewBag.SellerReplies = sellerReplies;
    //            var replayTimeDict = db.RatingReplaies
    //.Where(r => ratingIds.Contains(r.RatingId.Value))
    //.GroupBy(r => r.RatingId.Value)
    //.ToDictionary(g => g.Key, g => g.FirstOrDefault()?.ReplayTime); // 修正此处获取回复时间
    //            ViewBag.ReplayTime = replayTimeDict;
    //            // 計算評分平均星級數
    //            double averageStarRating = (double)(ratingsList.Any() ? ratingsList.Average(r => r.StarRating) : 0);
    //            ViewBag.AverageStarRating = averageStarRating;
    //            // 將評分按星級分類
    //            var allStarRatings = ratingsList;
    //            var fiveStarRatings = ratingsList.Where(r => r.StarRating == 5).ToList();
    //            var fourStarRatings = ratingsList.Where(r => r.StarRating == 4).ToList();
    //            var threeStarRatings = ratingsList.Where(r => r.StarRating == 3).ToList();
    //            var twoStarRatings = ratingsList.Where(r => r.StarRating == 2).ToList();
    //            var oneStarRatings = ratingsList.Where(r => r.StarRating == 1).ToList();

    //            // 計算評分數量
    //            int allStarCount = ratingsList.Count();
    //            int fiveStarCount = fiveStarRatings.Count();
    //            int fourStarCount = fourStarRatings.Count();
    //            int threeStarCount = threeStarRatings.Count();
    //            int twoStarCount = twoStarRatings.Count();
    //            int oneStarCount = oneStarRatings.Count();

    //            ViewBag.AllStarRatings = allStarRatings;
    //            ViewBag.FiveStarRatings = fiveStarRatings;
    //            ViewBag.FourStarRatings = fourStarRatings;
    //            ViewBag.ThreeStarRatings = threeStarRatings;
    //            ViewBag.TwoStarRatings = twoStarRatings;
    //            ViewBag.OneStarRatings = oneStarRatings;

    //            ViewBag.AllStarCount = allStarCount;
    //            ViewBag.FiveStarCount = fiveStarCount;
    //            ViewBag.FourStarCount = fourStarCount;
    //            ViewBag.ThreeStarCount = threeStarCount;
    //            ViewBag.TwoStarCount = twoStarCount;
    //            ViewBag.OneStarCount = oneStarCount;

    //            return View(ratingsList);
    //        }
    //    }

    //    private Dictionary<int, string> GetSellerReplies(List<int> ratingIds)
    //    {
    //        using (var db = new AppDbContext()) // 假設您的 DbContext 名稱為 AppDbContext
    //        {
    //            // 根據評分的 RatingId 查詢 RatingReply 的 ReplayText，並存儲在 Dictionary 中
    //            var sellerReplies = db.RatingReplaies
    //        .Where(r => ratingIds.Contains(r.RatingId ?? 0)) // 使用 ?? 0 處理可能為空值的情況
    //        .ToDictionary(r => r.RatingId ?? 0, r => r.ReplayText); // 使用 ?? 0 處理可能為空值的情況

    //            return sellerReplies;
    //        }
    //    }

    //    private List<RatingVM> GetRatingsList()
    //    {
    //        using (var db = new AppDbContext()) // 假設您的 DbContext 名稱為 AppDbContext
    //        {
    //            // 從資料庫中查詢評分資料，假設評分資料存在 Ratings 表中
    //            List<RatingVM> ratingsList = db.Ratings
    //                .Select(r => new RatingVM
    //                {
    //                    RatingId = r.RatingId,
    //                    StarRating = (int)r.StarRating,
    //                    RatingText = r.RatingText,
    //                    PostTime = (DateTime)r.PostTime
    //                })
    //                .ToList();

    //            return ratingsList;
    //        }
    //    }

    }
}