using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class SellerReplyController : Controller
    {
        // GET: SellerReply
        private readonly AppDbContext db = new AppDbContext();


        public ActionResult SellerReply()
        {
            int currentSellerId = (int)Session["SellerId"];
            using (var db = new AppDbContext())
            {
                // 获取 Rating 数据列表
                var productIds = db.Products
            .Where(product => product.SellerId == currentSellerId)
            .Select(product => product.ProductId)
            .ToList();
                // 取得 Rating 資料列表，並只選擇與目前賣家的ProductIds相關的留言
                var ratings = db.Ratings
                    .Where(rating => productIds.Contains((int)rating.ProductId.Value))
                    .ToList();
                // 将 Rating 数据转换为 UserFeedbackVM 类型的列表
                var feedbacks = ratings.Select(rating => new prjEShopping.Models.ViewModels.UserFeedbackVM
                {
                    RatingId = rating.RatingId,
                    UserId = rating.UserId,
                    StarRating = (int)rating.StarRating,
                    RatingText = rating.RatingText,
                    PostTime = rating.PostTime
                    // 在此处添加其他属性的转换，如果有的话
                }).ToList();

                // 将转换后的 UserFeedbackVM 数据传递给视图
                return View(feedbacks);
            }
        }
        [HttpPost]
        public ActionResult AddReply(int commentId, int starRating, string ratingText, string replyText)
        {
            //先寫死productId=1;
            //int userId = 1;
            // 在這裡將回覆資料儲存到 RatingReplaies 資料庫
            // 建立 RatingReplaies 物件並填入資料
            var ratingReply = new RatingReplay
            {
                RatingId = commentId,
                ReplayText = replyText,
                ReplayTime = DateTime.Now,
                ReplayStatus = "公開" // 假設你想要新增回覆時預設為"Pending"
            };

            // 執行將 ratingReply 物件新增至 RatingReplaies 資料表的程式碼，這裡假設使用 Entity Framework
            db.RatingReplaies.Add(ratingReply); // 將回覆資料加入資料庫
            db.SaveChanges(); // 儲存變更
             
            //// 同時將ProductId和BuyerId填入Rating資料表中
            //var rating = db.Ratings.FirstOrDefault(r => r.RatingId == commentId);
            //if (rating != null)
            //{
            //    rating.ProductId = productId;
            //    //rating.UserId = userId;之後有值要改
            //    db.SaveChanges();
            //}
            TempData["ReplySuccess"] = "回覆成功";
            // 執行完儲存後，你可以將使用者重新導向到回覆頁面或是刷新當前頁面
            // 這裡假設重新導向至回覆頁面
            return RedirectToAction("SellerReply", "SellerReply");
        }
    }
}