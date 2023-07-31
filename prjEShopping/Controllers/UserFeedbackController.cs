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
            UserFeedbackVM model = new UserFeedbackVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult SubmitComment(UserFeedbackVM model)
        {
            var db=new AppDbContext();
            var feedback = new Rating
            {
                UserId = model.UserId,
                ProductId = model.ProductId,
                StarRating = model.StarRating.ToString(),
                RatingText = model.RatingText,
                PostTime = DateTime.Now
            };
            db.Ratings.Add(feedback);
            db.SaveChanges();

            // 跳轉到其他頁面或返回成功消息
            return RedirectToAction("Index");
        }
       
    }

}
