using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;

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

        //優惠券列表分頁專用
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            // 連資料庫
            using (db)
            {
                int totalItems = db.Coupons.Count();
                //Skip(page - 1) * pageSize records and Take pageSize records
                var items = db.Coupons
                              .OrderBy(c => c.CouponId)  // 依照Id排序
                              .Skip((page - 1) * pageSize)
                              .Take(pageSize);

                var vm = items.Select(c => new CouponVM
                {
                    CouponId = c.CouponId,
                    SellerId = c.SellerId,
                    CouponNumber = c.CouponNumber,
                    CouponName = c.CouponName,
                    CouponDetails = c.CouponDetails,
                    Quantity = c.Quantity,
                    ReceivedQuantity = c.ReceivedQuantity,
                    CouponTerms = c.CouponTerms,
                    CouponType = c.CouponType,
                    Discount = c.Discount,
                    Storewide = c.Storewide,
                    StartTime = c.StartTime,
                    ClaimDeadline = c.ClaimDeadline,
                    EndTime = c.EndTime,
                    EventStatus = c.EventStatus
                }).ToList();


                var model = new PaginatedViewModel<CouponVM>
                {
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),  //取整數
                    Items = vm  //優惠券
                };

                //重要 要分開
                if (Request.IsAjaxRequest())
                {
                    // 如果是 AJAX 请求，只返回包含表格和分頁的視圖
                    return PartialView("_IndexCoupons", model);
                }
                else
                {
                    // 如果不是 AJAX 请求，返回包含布局(Layout)的完整頁面
                    return View(model);
                }
            }
        }

        //todo 優惠券列表分頁VM未搬家
        //優惠券分頁的VM 之後再搬家
        public class PaginatedViewModel<CouponVM>
        {
            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }
            public List<CouponVM> Items { get; set; }
        }

        // GET: Coupons/Create

        //todo 建立欄位商家專屬優惠券未連結商家資料庫
        //todo 優惠折扣內容選取欄位未變更 (選取免運 條件變成免運 選取打折 95折 9折 5折 或輸入數字+折字 選取抵扣 抵300 抵500 或填寫數字等)

        // GET: Coupons/All
        public ActionResult CouponsAll(int page = 1, int pageSize = 10)
        {
            using (db)
            {

                int totalItems = db.Coupons.Count();
                //Skip(page - 1) * pageSize records and Take pageSize records
                var items = db.Coupons
                              .OrderBy(c => c.CouponId)  // 依照Id排序
                              .Skip((page - 1) * pageSize)
                              .Take(pageSize);

                var vm = items.Select(c => new CouponVM
                {
                    CouponId = c.CouponId,
                    SellerId = c.SellerId,
                    CouponNumber = c.CouponNumber,
                    CouponName = c.CouponName,
                    CouponDetails = c.CouponDetails,
                    Quantity = c.Quantity,
                    ReceivedQuantity = c.ReceivedQuantity,
                    CouponTerms = c.CouponTerms,
                    CouponType = c.CouponType,
                    Discount = c.Discount,
                    Storewide = c.Storewide,
                    StartTime = c.StartTime,
                    ClaimDeadline = c.ClaimDeadline,
                    EndTime = c.EndTime,
                    EventStatus = c.EventStatus
                }).ToList();


                var model = new PaginatedViewModel<CouponVM>
                {
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),  //取整數
                    Items = vm  //優惠券
                };

                // 如果是 AJAX 请求，只返回包含表格和分頁的視圖
                return PartialView("_IndexCoupons", model);
            }
        }


        // GET: Coupons/CouponsOpening
        public ActionResult CouponsOpening(int page = 1, int pageSize = 10)
        {
            using (db)
            {
                var openCoupons = db.Coupons.Where(c => c.EndTime > DateTime.Now);
                int totalItems = openCoupons.Count();
                var items = openCoupons
                   .OrderBy(c => c.CouponId)
                   .Skip((page - 1) * pageSize)
                   .Take(pageSize);

                var vm = items.Select(c => new CouponVM
                {
                    CouponId = c.CouponId,
                    SellerId = c.SellerId,
                    CouponNumber = c.CouponNumber,
                    CouponName = c.CouponName,
                    CouponDetails = c.CouponDetails,
                    Quantity = c.Quantity,
                    ReceivedQuantity = c.ReceivedQuantity,
                    CouponTerms = c.CouponTerms,
                    CouponType = c.CouponType,
                    Discount = c.Discount,
                    Storewide = c.Storewide,
                    StartTime = c.StartTime,
                    ClaimDeadline = c.ClaimDeadline,
                    EndTime = c.EndTime,
                    EventStatus = c.EventStatus
                }).ToList();

                var model = new PaginatedViewModel<CouponVM>
                {
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                    Items = vm
                };

                //return new EmptyResult();
                return PartialView("_IndexCoupons", model);

            }
        }

        public ActionResult CouponsClosing(int page = 1, int pageSize = 10)
        {
            using (db)
            {
                var openCoupons = db.Coupons.Where(c => c.EndTime < DateTime.Now);
                int totalItems = openCoupons.Count();
                var items = openCoupons
                   .OrderBy(c => c.CouponId)
                   .Skip((page - 1) * pageSize)
                   .Take(pageSize)
                   .ToList();

                var vm = items.Select(c => new CouponVM
                {
                    CouponId = c.CouponId,
                    SellerId = c.SellerId,
                    CouponNumber = c.CouponNumber,
                    CouponName = c.CouponName,
                    CouponDetails = c.CouponDetails,
                    Quantity = c.Quantity,
                    ReceivedQuantity = c.ReceivedQuantity,
                    CouponTerms = c.CouponTerms,
                    CouponType = c.CouponType,
                    Discount = c.Discount,
                    Storewide = c.Storewide,
                    StartTime = c.StartTime,
                    ClaimDeadline = c.ClaimDeadline,
                    EndTime = c.EndTime,
                    EventStatus = c.EventStatus
                }).ToList();

                var model = new PaginatedViewModel<CouponVM>
                {
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                    Items = vm
                };

                //return new EmptyResult();
                return PartialView("_IndexCoupons", model);

            }
        }

        public ActionResult Create()
        {
            List<string> CouponsNums = db.Coupons.Select(a => a.CouponNumber).ToList();

            var date = DateTime.Now;
            string year = date.Year.ToString().Substring(2, 2);
            string month = date.Month.ToString().PadLeft(2, '0');

            int count = 1;
            string CouponNum = GetCouponNums(year, month, count);

            while (CouponsNums.Contains(CouponNum))
            {
                count++;
                CouponNum = GetCouponNums(year, month, count);
            }

            ViewBag.CouponNum = CouponNum;

            var SellerName=db.Sellers.Select(x=>x.SellerName).ToList();

            ViewBag.SellerName = SellerName;

            return View();
        }

        public string GetCouponNums(string year, string month,int count)
        {
            return $"C{year}{month}{count.ToString().PadLeft(2, '0')}";
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
