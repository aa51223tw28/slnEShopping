using prjEShopping.Models.DTOs;
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

        public ActionResult Index()
        {
            int? userId = (int?)Session["UserId"];
            var productDto = new UserProductIndexDto();
            var feedbackVm = new UserFeedbackVM
            {
                UserId = userId
            };
           
            ViewData["ProductDto"] = productDto;
            ViewData["FeedbackVm"] = feedbackVm;

            return View();
        }
        [HttpPost]
        public ActionResult SubmitComment(UserFeedbackVM model)
        {
            if (ModelState.IsValid)
            {
                var db = new AppDbContext();
                var feedback = new Rating
                {
                    UserId = model.UserId,
                    ProductId = model.ProductId,
                    StarRating = model.StarRating,
                    RatingText = model.RatingText,
                    PostTime = DateTime.Now
                };

                db.Ratings.Add(feedback);
                db.SaveChanges();

                // 跳转到其他页面或返回成功消息
                return RedirectToAction("Index");
            }
            else
            {
                // 如果模型验证失败，重新加载视图并显示验证错误信息
                return View("Index", model);
            }
        }
    }
}
