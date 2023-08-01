using prjEShopping.Models.EFModels;
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
            using (var db = new AppDbContext())
            {
                // 获取 Rating 数据列表
                var ratings = db.Ratings.ToList();

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
    }
}