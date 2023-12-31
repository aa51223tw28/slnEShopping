﻿using prjEShopping.Models.DTOs;
using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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

            var discountedProductIds=db.ADProducts.Select(x => x.ProductId).ToList();

            foreach (var item in products)
            {
                var orderQuantity = db.ProductStocks.Where(x => x.ProductId == item.ProductId).Select(x => x.OrderQuantity).FirstOrDefault() ?? 0;

                var stockQuantity = db.ProductStocks.Where(x => x.ProductId == item.ProductId).Select(x => x.StockQuantity).FirstOrDefault() ?? 0;

                //商品狀態 1審核中/2販售中/3下架中
                var productstatus = products.Where(x => x.ProductId == item.ProductId).Select(x => x.ProductStatusId).FirstOrDefault();


                if (!discountedProductIds .Contains(item.ProductId)&& stockQuantity - orderQuantity > 0&& productstatus==2)
                {
                    var data = new UserProductIndexDto()
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Price = (decimal)item.Price,
                        ProductImagePathOne = item.ProductImagePathOne,
                        ProductStock = stockQuantity - orderQuantity,
                        QuantitySold = (int)db.ProductStocks.FirstOrDefault(x => x.ProductId == item.ProductId).QuantitySold,
                        SubcategoryId = (int)products.FirstOrDefault(x => x.ProductId == item.ProductId).SubcategoryId,
                        BrandId = (int)item.BrandId,
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
            var orderQuantity = db.ProductStocks.Where(x => x.ProductId == productId).Select(x => x.OrderQuantity).FirstOrDefault() ?? 0;
            var stockQuantity = db.ProductStocks.Where(x => x.ProductId == productId).Select(x => x.StockQuantity).FirstOrDefault() ?? 0;

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
            var optionNameTwo = db.ProductOptions.Where(x => x.OptionId == parsedOptionIdTwo).Select(x => x.OptionName).FirstOrDefault();
            var optionNameThree = db.ProductOptions.Where(x => x.OptionId == parsedOptionIdThree).Select(x => x.OptionName).FirstOrDefault();
            var optionNameFour = db.ProductOptions.Where(x => x.OptionId == parsedOptionIdFour).Select(x => x.OptionName).FirstOrDefault();
            var optionNameFive = db.ProductOptions.Where(x => x.OptionId == parsedOptionIdFive).Select(x => x.OptionName).FirstOrDefault();

            //選項名稱
            var SpecificationIdOne = db.ProductOptions.Where(x => x.OptionName == optionNameOne).Select(x => x.SpecificationId).FirstOrDefault();
            var SpecificationIdTwo = db.ProductOptions.Where(x => x.OptionName == optionNameTwo).Select(x => x.SpecificationId).FirstOrDefault();
            var SpecificationIdThree = db.ProductOptions.Where(x => x.OptionName == optionNameThree).Select(x => x.SpecificationId).FirstOrDefault();
            var SpecificationIdFour = db.ProductOptions.Where(x => x.OptionName == optionNameFour).Select(x => x.SpecificationId).FirstOrDefault();
            var SpecificationIdOFive = db.ProductOptions.Where(x => x.OptionName == optionNameFive).Select(x => x.SpecificationId).FirstOrDefault();

            var SpecificationNameOne = db.ProductSpecifications.Where(x => x.SpecificationId == SpecificationIdOne).Select(x => x.SpecificationName).FirstOrDefault();
            var SpecificationNameTwo = db.ProductSpecifications.Where(x => x.SpecificationId == SpecificationIdTwo).Select(x => x.SpecificationName).FirstOrDefault();
            var SpecificationNameThree = db.ProductSpecifications.Where(x => x.SpecificationId == SpecificationIdThree).Select(x => x.SpecificationName).FirstOrDefault();
            var SpecificationNameFour = db.ProductSpecifications.Where(x => x.SpecificationId == SpecificationIdFour).Select(x => x.SpecificationName).FirstOrDefault();
            var SpecificationNameFive = db.ProductSpecifications.Where(x => x.SpecificationId == SpecificationIdOFive).Select(x => x.SpecificationName).FirstOrDefault();

            //大分類名稱
            var categoryId = db.Products.Where(x => x.ProductId == productId).Select(x => x.CategoryId).FirstOrDefault();
            var categoryName = db.ProductMainCategories.Where(x => x.CategoryId == categoryId).Select(x => x.CategoryName).FirstOrDefault();

            //小分類名稱
            var subcategoryId = db.Products.Where(x => x.ProductId == productId).Select(x => x.SubcategoryId).FirstOrDefault();
            var subcategoryName = db.ProductSubCategories.Where(x => x.SubcategoryId == subcategoryId).Select(x => x.SubcategoryName).FirstOrDefault();

            var datas = new UserProductIndexDto()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Price = (decimal)product.Price,
                ProductImagePathOne = product.ProductImagePathOne,
                ProductImagePathTwo = product.ProductImagePathTwo,
                ProductImagePathThree = product.ProductImagePathThree,
                ProductStock = stockQuantity - orderQuantity,
                QuantitySold = (int)quantitySold,

                //一堆規格
                BrandName = brandName,
                SpecificationNameOne = SpecificationNameOne,
                SpecificationNameTwo = SpecificationNameTwo,
                SpecificationNameThree = SpecificationNameThree,
                SpecificationNameFour = SpecificationNameFour,
                SpecificationNameFive = SpecificationNameFive,

                OptionNameOne = optionNameOne,
                OptionNameTwo = optionNameTwo,
                OptionNameThree = optionNameThree,
                OptionNameFour = optionNameFour,
                OptionNameFive = optionNameFive,

                CategoryName = categoryName,
                SubcategoryName = subcategoryName,

                
                Discount = db.ADProducts.FirstOrDefault(ad => ad.ProductId == product.ProductId)?.Discount ?? 0,
                //要判斷折價商品的時間
                DiscountPrice = IsInDiscountPeriod((int)product.ProductId) ? (decimal)product.Price * (db.ADProducts.FirstOrDefault(ad => ad.ProductId == product.ProductId)?.Discount ?? 0) / 100 : (decimal)product.Price,
            };
            //現在購物車該使用者該購物車id該商品的數量
            var qua = db.ShoppingCartDetails.Where(x => x.CartId == cartid && x.ProductId == productId).Select(x => x.Quantity).FirstOrDefault();
            if (qua != null)
            {
                ViewBag.TotalQuantity = qua;
            }
            else
            {
                ViewBag.TotalQuantity = 0;
            }
            ViewBag.SupportNum = GenerateSupportNumberU();
            ViewBag.UserId = userid;

            return View(datas);
        }


        private bool IsInDiscountPeriod(int productId)//判斷折價價格期限
        {
            var now = DateTime.Now;
            var db = new AppDbContext();
            return db.ADProducts.Any(ad => ad.ProductId == productId && ad.ADStartDate <= now && ad.ADEndDate >= now);
        }

        [HttpPost]
        //客戶端寄信
        public ActionResult CSSendMail(SupportVM vm)
        {
            var db = new AppDbContext();
            if (vm != null)
            {   //存圖..
                imageAdd(vm);
                var s = new Support();
                s = SupportChange.VM2Support(vm);
                db.Supports.Add(s);
                db.SaveChanges();
                return new EmptyResult();
            }
            return View(vm);
        }


        public string GenerateSupportNumberU()
        {
            var db = new AppDbContext();
            var today = DateTime.Now;
            // 獲取當前日期的格式化字符串 "230812"
            var datePart = today.ToString("yyMMdd");

            // 查詢當天已有多少條記錄
            var countForToday = db.Supports
                                  .Where(s => s.SupportNumber.StartsWith("US" + datePart))
                                  .Count();
            // 生成下一個編號
            var numberPart = (countForToday + 1).ToString("D4"); // 四位數，不足前面補0

            return "US" + datePart + numberPart;
        }

        //存圖範例
        private void imageAdd(SupportVM vm)
        {
            if (vm.ImageFile != null && vm.ImageFile.ContentLength > 0)
            {
                // 生成一個唯一的檔案名稱
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.ImageFile.FileName);

                // 指定檔案的保存路徑
                var path = Path.Combine(Server.MapPath("~/img/"), fileName);

                // 儲存檔案
                vm.ImageFile.SaveAs(path);

                // 將檔案名稱保存到Support模型的相應欄位
                vm.ImageLink = fileName;
            }
        }


        [Authorize]
        public ActionResult TrackProductapi(int productId)//在商品頁面追蹤什麼商品
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();

            var trackproductId = db.TrackProducts.FirstOrDefault(x => x.ProductId == productId && x.UserId == userid);

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
                var trackProductToDelete = db.TrackProducts.FirstOrDefault(x => x.TrackProductId == trackProductIdToDelete && x.ProductId == productId);
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

            var trackproductId = db.TrackProducts.FirstOrDefault(x => x.ProductId == productId && x.UserId == userid);

            if (trackproductId == null)
            {
                return Content("noTrackProduct");
            }
            else
            {
                return Content("TrackProduct");
            }
        }

        //評論api
        public ActionResult Comment(int productId)
        {
            using (var db = new AppDbContext())
            {
                var customerAccount = User.Identity.Name;
                var userphoto = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserImagePath).FirstOrDefault();

                var ratingsList = (from x in db.Ratings
                                   join y in db.RatingReplaies
                                   on x.RatingId equals y.RatingId into grouping
                                   from g in grouping.DefaultIfEmpty()
                                   join u in db.Users
                                   on x.UserId equals u.UserId // Assuming there's a UserId field in Ratings table
                                   where x.ProductId == productId
                                   select new RatingDateStringVM
                                   {
                                       RatingId = x.RatingId,
                                       StarRating = (int)x.StarRating,
                                       RatingText = x.RatingText,
                                       PostTime = x.PostTime.ToString(),
                                       ReplayText = g.ReplayText,
                                       ReplayTime = g.ReplayTime.ToString(),
                                       UserImagePath = u.UserImagePath // Assuming there's a UserImagePath field in Users table
                                   })
                                   .ToList();
                return Json(ratingsList, JsonRequestBehavior.AllowGet);
            }
        }
    

        private Dictionary<int, string> GetSellerReplies(List<int> ratingIds)
        {
            using (var db = new AppDbContext()) 
            {
                // 根據評分的 RatingId 查詢 RatingReply 的 ReplayText，並存儲在 Dictionary 中
                var sellerReplies = db.RatingReplaies
            .Where(r => ratingIds.Contains(r.RatingId ?? 0)) // 使用 ?? 0 處理可能為空值的情況
            .ToDictionary(r => r.RatingId ?? 0, r => r.ReplayText); // 使用 ?? 0 處理可能為空值的情況

                return sellerReplies;
            }
        }

        private List<RatingVM> GetRatingsList()
        {
            using (var db = new AppDbContext()) // 假設您的 DbContext 名稱為 AppDbContext
            {
                // 從資料庫中查詢評分資料，假設評分資料存在 Ratings 表中
                List<RatingVM> ratingsList = db.Ratings
                    .Select(r => new RatingVM
                    {
                        RatingId = r.RatingId,
                        StarRating = (int)r.StarRating,
                        RatingText = r.RatingText,
                        PostTime = (DateTime)r.PostTime
                    })
                    .ToList();

                return ratingsList;
            }
        }

    }
}