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
        public ActionResult Index()
        {
            HttpCookie authCookie = Request.Cookies["AdminLogin"];
            if (authCookie == null || authCookie.Values["status"] != "AdminLogin" || authCookie.Values["AccessRightId"] != "1")
            {
                return RedirectToAction("Login", "Admins");
            }
            string decodedName = HttpUtility.UrlDecode(authCookie.Values["userName"]);
            ViewBag.AdminName = decodedName;

            var model = db.Coupons.Coupon2VM();
            return View(model);
        }

        public ActionResult CouponListForSeller(int? SellerId)
        { //等接商家頁面
            SellerId = 1;
            if (SellerId != 0)
            {
                var model = db.Coupons.Where(x => x.SellerId == SellerId).Coupon2VM();
                return View(model);
            }

            return RedirectToAction("Index","Main");
        }

        // GET: Coupons/Create
        public ActionResult Create()
        {
            HttpCookie authCookie = Request.Cookies["AdminLogin"];
            if (authCookie == null || authCookie.Values["status"] != "AdminLogin" || authCookie.Values["AccessRightId"] != "1")
            {
                return RedirectToAction("Login", "Admins");
            }
            string decodedName = HttpUtility.UrlDecode(authCookie.Values["userName"]);
            ViewBag.AdminName = decodedName;

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

            var Sellers = db.Sellers.Select(x => x.StoreName).ToList();

            ViewBag.StoreName = Sellers;

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
        public ActionResult Create([Bind(Include = "CouponId,SellerId,CouponNumber,CouponName,CouponDetails,Quantity,ReceivedQuantity,CouponTerms,CouponType,Discount,Storewide,StartTime,ClaimDeadline,EndTime,EventStatus")] CouponVM vm)
        {
            int _sellerId =0;
            if (vm.Storewide != "全館")
                _sellerId = db.Sellers.Where(s => s.StoreName == vm.Storewide).Select(s => s.SellerId).FirstOrDefault();

            var coupon = new Coupon
            {
                CouponId = vm.CouponId,
                SellerId = _sellerId,
                CouponNumber = vm.CouponNumber,
                CouponName = vm.CouponName,
                CouponDetails = vm.CouponDetails,
                Quantity = vm.Quantity,
                ReceivedQuantity = 0,
                CouponTerms = vm.CouponTerms,
                CouponType = vm.CouponType,
                Discount = vm.Discount,
                Storewide = vm.Storewide,
                StartTime = vm.StartTime,
                ClaimDeadline = vm.ClaimDeadline,
                EndTime = vm.EndTime,
                EventStatus = vm.EventStatus
            };

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
            HttpCookie authCookie = Request.Cookies["AdminLogin"];
            if (authCookie == null || authCookie.Values["status"] != "AdminLogin" || authCookie.Values["AccessRightId"] != "1")
            {
                return RedirectToAction("Login", "Admins");
            }
            string decodedName = HttpUtility.UrlDecode(authCookie.Values["userName"]);
            ViewBag.AdminName = decodedName;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coupon c = db.Coupons.Find(id);
            if (c == null)
            {
                return HttpNotFound();
            }

            var vm = new CouponVM
            {
                CouponId =(int)c.CouponId,
                SellerId =(int)c.SellerId,
                CouponNumber = c.CouponNumber,
                CouponName = c.CouponName,
                CouponDetails = c.CouponDetails,
                Quantity =(int) c.Quantity,
                ReceivedQuantity =(int)c.ReceivedQuantity,
                CouponTerms = c.CouponTerms,
                CouponType = c.CouponType,
                Discount = c.Discount,
                Storewide = c.Storewide,
                StartTime = c.StartTime,
                ClaimDeadline = c.ClaimDeadline,
                EndTime = c.EndTime,
                EventStatus = c.EventStatus
            };

            return View(vm);
        }

        // POST: Coupons/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CouponId,SellerId,CouponNumber,CouponName,CouponDetails,Quantity,ReceivedQuantity,CouponTerms,CouponType,Discount,Storewide,StartTime,ClaimDeadline,EndTime,EventStatus")] CouponVM vm)
        {
            var coupon = new Coupon
            {
                CouponId =(int) vm.CouponId,
                SellerId =(int) vm.SellerId,
                CouponNumber = vm.CouponNumber,
                CouponName = vm.CouponName,
                CouponDetails = vm.CouponDetails,
                Quantity = (int)vm.Quantity,
                ReceivedQuantity =(int)vm.ReceivedQuantity,
                CouponTerms = vm.CouponTerms,
                CouponType = vm.CouponType,
                Discount = vm.Discount,
                Storewide = vm.Storewide,
                StartTime = vm.StartTime,
                ClaimDeadline = vm.ClaimDeadline,
                EndTime = vm.EndTime,
                EventStatus = vm.EventStatus
            };

            if (ModelState.IsValid)
            {
                db.Entry(coupon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(coupon);
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
