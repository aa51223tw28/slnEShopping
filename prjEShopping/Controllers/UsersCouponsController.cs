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

        //未領取優惠券列表
        public ActionResult List(int? userId)
        {
            //todo 要判定總數量是否<0 
            //先預設
            userId = 1;
            var _coupons = db.Coupons
                .Where(x => x.StartTime < DateTime.Now && x.EndTime > DateTime.Now)
                .Select(c => c.CouponId);

            // 找出已有的CouponId
            var usersCouponIds = db.UsersCoupons
                .Where(u => u.UserId == userId)  // 用你的用户ID替换这里
                .Select(uc => uc.CouponId.Value);

            // 剔除相同的優惠券
            var couponsToDisplay = _coupons.Except(usersCouponIds);

            // 取得的剩下的優惠券
            var model = db.Coupons
                .Where(c => couponsToDisplay.Contains(c.CouponId) && c.EventStatus == "open")
                .ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult List(int userId, int couponId)
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

        //已領取優惠券列表
        public ActionResult UsersCouponsList(int? userId)
        {
            userId = 1;
            var usersCouponIds = db.UsersCoupons.Where(uc => uc.UserId == userId && uc.CouponStatus == "可使用").Select(uc => uc.CouponId);

            var model = db.Coupons
                          .Where(c => usersCouponIds.Contains(c.CouponId) && c.EndTime > DateTime.Now)
                          .ToList();

            return View(model);
        }

        //已使用優惠券列表
        public ActionResult UsersCouponsUsed(int? userId)
        {
            userId = 1;
            var usersCouponIds = db.UsersCoupons.Where(uc => uc.UserId == userId && uc.CouponStatus == "已使用").Select(uc => uc.CouponId);

            var model = db.Coupons
                          .Where(c => usersCouponIds.Contains(c.CouponId))
                          .ToList();

            return View(model);
        }

        //已逾期優惠券列表
        public ActionResult UsersCouponsOverdue(int? userId)
        {
            userId = 1;
            var usersCouponIds = db.UsersCoupons.Where(uc => uc.UserId == userId && uc.CouponStatus == "可使用").Select(uc => uc.CouponId);

            var model = db.Coupons
                          .Where(c => usersCouponIds.Contains(c.CouponId) && c.EndTime < DateTime.Now)
                          .ToList();

            return View(model);
        }

    }
}