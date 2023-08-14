using prjEShopping.Models.DTOs;
using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class UserFeedbackController : Controller
    {

        public ActionResult Index(int productId)
        {
            //  int? userId = (int?)Session["UserId"];

            //讀取登入帳號(電子郵件)
            var customerAccount = User.Identity.Name;
            //根據登入帳號取得帳號ID
            var db = new AppDbContext();
            var userId = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var productName = db.Products.Where(p => p.ProductId == productId).Select(p => p.ProductName).FirstOrDefault();

            var productDto = new UserProductIndexDto();
            var feedbackVm = new UserFeedbackVM
            {
                UserId = userId
            };
           
            ViewData["ProductDto"] = productDto;
            ViewData["FeedbackVm"] = feedbackVm;
            ViewData["ProductName"] = productName;
            return View(feedbackVm);
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
