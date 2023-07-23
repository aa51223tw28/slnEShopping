using prjEShopping.Models.EFModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class UsersCouponsController : Controller
    {
        private AppDbContext db = new AppDbContext();
        // GET: UsersCoupons
        public ActionResult List(int? userId)
        {
            //先預設
            userId = 1;
            var _coupons = db.Coupons
                .Where(x => x.StartTime < DateTime.Now && x.EndTime > DateTime.Now)
                .Select(c => c.CouponId);

            // 找出已有的CouponId
            var usersCouponIds = db.UsersCoupons
                .Where(u => u.UserId == userId)  // 用你的用户ID替换这里
                .Select(uc => uc.CouponId.Value);

            // 使用Except操作，从activeCoupons中剔除用户已有的couponId
            var couponsToDisplay =_coupons.Except(usersCouponIds);

            // 获取相应的Coupon对象
            var model = db.Coupons
                .Where(c => couponsToDisplay.Contains(c.CouponId))
                .ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult List(int userId,int couponId)
        {
            UsersCoupon usersCoupon = new UsersCoupon()
            {
                UserId = userId,
                CouponId = couponId,
                GetDate = DateTime.Now,
                CouponStatus = "可使用"
            };

            if (ModelState.IsValid)
            {
                db.UsersCoupons.Add(usersCoupon);
                db.SaveChanges();
                return RedirectToAction("List");
            }
         
            return View(usersCoupon);
        }
    }
}