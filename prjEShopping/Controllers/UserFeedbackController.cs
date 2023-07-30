using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class UserFeedbackController : Controller
    {
        private readonly AppDbContext db = new AppDbContext();

        public ActionResult Index(int sellerId)
        {
            var seller = db.Sellers.FirstOrDefault(s => s.SellerId == sellerId);
            if (seller == null)
            {
                // Seller not found, handle accordingly (e.g., show error page)
                return HttpNotFound();
            }

            var viewModel = new UserFeedbackVM
            {
                SellerId = sellerId,
                SellerName = seller.SellerName,
                Ratings = db.Ratings.Where(r => r.ProductId == sellerId && r.RatingText != null).ToList(),
                Comments = db.Ratings.Where(c => c.ProductId == sellerId && c.RatingText == null).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddRating(int sellerId, int ratingValue, string ratingText)
        {
            var userId = 1; // Replace this with the actual userId of the buyer (you may get it from the session or authentication)
            var rating = new Rating
            {
                ProductId = sellerId,
                UserId = userId,
                StarRating = ratingValue.ToString(),
                RatingText = ratingText,
                PostTime = DateTime.Now,
                //RatingStatus = 1 // You may set the initial status of the rating
            };

            db.Ratings.Add(rating);
            db.SaveChanges();

            return RedirectToAction("Index", new { sellerId });
        }

        [HttpPost]
        public ActionResult AddComment(int sellerId, string commentText)
        {
            var userId = 1; // Replace this with the actual userId of the buyer (you may get it from the session or authentication)
            var comment = new Rating
            {
                ProductId = sellerId,
                UserId = userId,
                RatingText = commentText,
                PostTime = DateTime.Now,
                //RatingStatus = 1 // You may set the initial status of the comment
            };

            db.Ratings.Add(comment);
            db.SaveChanges();

            return RedirectToAction("Index", new { sellerId });
        }
        public ActionResult ViewFeedback(int sellerId)
        {
            var viewModel = new UserFeedbackVM();

            // 從資料庫中取得賣家資訊，這裡假設你有一個名為AppDbContext的DbContext類別
            var db = new AppDbContext();
            var seller = db.Sellers.FirstOrDefault(s => s.SellerId == sellerId);
            if (seller == null)
            {
                // 賣家不存在，顯示錯誤訊息或導向錯誤頁面
                return View("Error");
            }

            // 設定賣家資訊到ViewModel
            viewModel.SellerName = seller.SellerName;

            // 從資料庫中取得買家留言資料
            viewModel.Ratings = db.Ratings.Where(f => f.UserId == sellerId).ToList();

            return View(viewModel);
        }
    }

}
