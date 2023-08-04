using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class UserTestCommentController : Controller
    {
        // GET: UserTestComment
        public ActionResult Comment()
        {
            using (var db = new AppDbContext()) // 假設您的 DbContext 名稱為 AppDbContext
            {
                // 從資料庫中查詢評分資料，假設評分資料存在 Ratings 表中
                List<Rating> ratingsList = db.Ratings.ToList();

                // 取得所有評分對應的回覆資料
                var ratingIds = ratingsList.Select(r => r.RatingId);
                var sellerReplies = db.RatingReplaies
                    .Where(r => ratingIds.Contains(r.RatingId.Value))
                    .GroupBy(r => r.RatingId.Value) // 按照 RatingId 分組
                    .ToDictionary(g => g.Key, g => g.FirstOrDefault()?.ReplayText ?? string.Empty);
                ViewBag.SellerReplies = sellerReplies;
                // 計算評分平均星級數
                double averageStarRating = (double)(ratingsList.Any() ? ratingsList.Average(r => r.StarRating) : 0);
                ViewBag.AverageStarRating = averageStarRating;
                // 將評分按星級分類
                var allStarRatings = ratingsList;
                var fiveStarRatings = ratingsList.Where(r => r.StarRating == 5).ToList();
                var fourStarRatings = ratingsList.Where(r => r.StarRating == 4).ToList();
                var threeStarRatings = ratingsList.Where(r => r.StarRating == 3).ToList();
                var twoStarRatings = ratingsList.Where(r => r.StarRating == 2).ToList();
                var oneStarRatings = ratingsList.Where(r => r.StarRating == 1).ToList();

                // 計算評分數量
                int allStarCount = ratingsList.Count();
                int fiveStarCount = fiveStarRatings.Count();
                int fourStarCount = fourStarRatings.Count();
                int threeStarCount = threeStarRatings.Count();
                int twoStarCount = twoStarRatings.Count();
                int oneStarCount = oneStarRatings.Count();
                
                ViewBag.AllStarRatings = allStarRatings;
                ViewBag.FiveStarRatings = fiveStarRatings;
                ViewBag.FourStarRatings = fourStarRatings;
                ViewBag.ThreeStarRatings = threeStarRatings;
                ViewBag.TwoStarRatings = twoStarRatings;
                ViewBag.OneStarRatings = oneStarRatings;

                ViewBag.AllStarCount = allStarCount;
                ViewBag.FiveStarCount = fiveStarCount;
                ViewBag.FourStarCount = fourStarCount;
                ViewBag.ThreeStarCount = threeStarCount;
                ViewBag.TwoStarCount = twoStarCount;
                ViewBag.OneStarCount = oneStarCount;

                return View(ratingsList);
            }
        }

        private Dictionary<int, string> GetSellerReplies(List<int> ratingIds)
        {
            using (var db = new AppDbContext()) // 假設您的 DbContext 名稱為 AppDbContext
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