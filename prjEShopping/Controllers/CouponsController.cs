using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using prjEShopping.Models.EFModels;

namespace prjEShopping.Controllers
{
    public class CouponsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Coupons 原始設定
        //public ActionResult Index()
        //{

        //    return View(db.Coupons.ToList());
        //}

        //測試分頁用
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            // Assume db is your DbContext
            using (db)
            {
                int totalItems = db.Coupons.Count();
                // Skip (page - 1) * pageSize records and Take pageSize records
                var items = db.Coupons
                              .OrderBy(c => c.CouponId)  // Important to order the records before Skip and Take
                              .Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();

                var model = new PaginatedViewModel<Coupon>
                {
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                    Items = items
                };

                //return View(model);

                if (Request.IsAjaxRequest())
                {
                    // 如果是 AJAX 请求，只返回包含表格和分页链接的视图
                    return PartialView("_IndexCoupons", model);
                }
                else
                {
                    // 如果不是 AJAX 请求，返回包含布局的完整页面
                    return View(model);
                }
            }
        }

        public class PaginatedViewModel<Coupon>
        {
            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }
            public List<Coupon> Items { get; set; }
        }

        // GET: Coupons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Coupons/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CouponId,CouponNumber,CouponName,CouponDetails,Quantity,CouponTerms,CouponType,Discount,Storewide,StartTime,ClaimDeadline,EndTime,EventStatus")] Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                db.Coupons.Add(coupon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(coupon);
        }

        // GET: Coupons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coupon coupon = db.Coupons.Find(id);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            return View(coupon);
        }

        // POST: Coupons/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CouponId,CouponNumber,CouponName,CouponDetails,Quantity,CouponTerms,CouponType,Discount,Storewide,StartTime,ClaimDeadline,EndTime,EventStatus")] Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(coupon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(coupon);
        }

        // GET: Coupons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coupon coupon = db.Coupons.Find(id);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            return View(coupon);
        }

        // POST: Coupons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Coupon coupon = db.Coupons.Find(id);
            db.Coupons.Remove(coupon);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
