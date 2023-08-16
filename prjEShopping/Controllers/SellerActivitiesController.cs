using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace prjEShopping.Controllers
{
    public class SellerActivitiesController : Controller
    {
        private AppDbContext db = new AppDbContext();
        // GET: SellerActivities
        public ActionResult ADList()
        { // 廣告-1 購買廣告列表
            int? sellerId = Session["SellerId"] as int?;
            if (sellerId == null)
            { return RedirectToAction("Login", "SellerLogin"); }
            else
            { sellerId = (int)Session["SellerId"]; }
            ViewBag.SellerId = sellerId;
            // 獲取所有在SellersAD中存在的ADProductId
            var existingADProductIds = db.SellersADs.Select(sa => sa.ADProductId).ToList();

            // 從ADProducts中獲取不在existingADProductIds中的產品
            var model = db.ADProducts
                         .Where(ap => !existingADProductIds.Contains(ap.ADProductId)&&ap.ADEndDate>DateTime.Now)
                         .ToList()
                         .ADProduct2VM(); //從ADProduct列表轉換為VM的方法

            ViewBag.SellerPoint = db.Sellers.Where(s => s.SellerId == sellerId).Select(p => p.ADPoints).FirstOrDefault();

            return View(model);
        }

        public ActionResult ADCreate(int adproductId)
        { //廣告-2-1 購買廣告確認頁面
            int? sellerId = Session["SellerId"] as int?;
            if (sellerId == null)
            { return RedirectToAction("Login", "SellerLogin"); }
            else
            { sellerId = (int)Session["SellerId"]; }

            var model = db.ADProducts.Where(a => a.ADProductId == adproductId).ADProduct2VM().FirstOrDefault();

            ViewBag.SellerId = sellerId;
            ViewBag.Products = db.Products.Where(s => s.SellerId == sellerId).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult ADCreate(ADProductVM vm, int SellerId)
        {//廣告-2-2 購買廣告資料庫紀錄
            if (ModelState.IsValid)
            {
                var ad = db.ADProducts.Where(x => x.ADProductId == vm.ADProductId).FirstOrDefault();
                if (db.SellersADs.Where(x => x.ADProductId == vm.ADProductId).FirstOrDefault() != null)
                    return RedirectToAction("ADList");

                if (ad != null)
                {
                    ADProductChange.UpdateAdProduct(ad, vm);

                    var sellersad = new SellersAD
                    {
                        SellerId = SellerId,
                        ADProductId = vm.ADProductId,
                    };
                    db.SellersADs.Add(sellersad);

                    var point = new ADPoint
                    {
                        SellerId = SellerId,
                        ADPoints = -(int)vm.ADPoint,
                        GUINumber = db.Sellers.Where(x => x.SellerId == SellerId).Select(x => x.GUINumber).FirstOrDefault(),
                        PaymentStatus = "0",
                        PurchaseTime = DateTime.Now,
                    };
                    db.ADPoints.Add(point);

                    var sellerToUpdate = db.Sellers.FirstOrDefault(x => x.SellerId == SellerId);

                    sellerToUpdate.ADPoints -= vm.ADPoint;
                    db.Entry(sellerToUpdate).State = EntityState.Modified; // 表明該實體的狀態已被修改

                    db.SaveChanges();
                    return RedirectToAction("ADOwner");
                }
                ViewBag.Products = db.Products.Where(s => s.SellerId == SellerId).ToList();
                return View(vm);
            }
            else
            {
                // 模型無效，需要返回相同的視圖，並附帶當前模型和任何必要的ViewBag數據
                ViewBag.Products = db.Products.Where(s => s.SellerId == SellerId).ToList();
                return View(vm);
            }
        }
        [HttpPost]
        public JsonResult UploadImage(HttpPostedFileBase ADImage)
        {//廣告-2-3 購買廣告存圖
            if (ADImage != null && ADImage.ContentLength > 0)
            {
                string fileName = Path.GetFileNameWithoutExtension(ADImage.FileName);
                string extension = Path.GetExtension(ADImage.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string pathToSave = Path.Combine(Server.MapPath("~/img/"), fileName);

                // 檢查目錄是否存在
                var directory = Path.GetDirectoryName(pathToSave);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                ADImage.SaveAs(pathToSave);

                // 如果你想通過URL訪問圖像，可能還需要返回相對路徑
                string relativePath = "/img/" + fileName;

                return Json(new { success = true, path = relativePath });
            }
            else
            {
                return Json(new { success = false, message = "文件上傳失敗" });
            }
        }
        public ActionResult ADOwner() 
        {//廣告-3 擁有廣告列表
            int? sellerId = Session["SellerId"] as int?;
            if (sellerId == null)
            { return RedirectToAction("Login", "SellerLogin"); }
            else
            { sellerId = (int)Session["SellerId"]; }

            var sellerAdProductIds = db.SellersADs
                           .Where(x => x.SellerId == sellerId)
                           .Select(x => x.ADProductId)
                           .ToList();

            var model = db.ADProducts
                               .Where(x => sellerAdProductIds.Contains(x.ADProductId))
                               .ADProduct2VM();

            return View(model);
        }
       
        public ActionResult CouponList()
        {
            int? sellerId = Session["SellerId"] as int?;
            if (sellerId == null)
            {
                sellerId = 1;
            }
                var model = db.Coupons.Where(x => x.SellerId == sellerId).Coupon2VM();
            var path=db.Sellers.Where(x => x.SellerId == sellerId).Select(p=>p.SellerImagePath).FirstOrDefault();
            ViewBag.ImagePath=path;
                return View(model);
        }

        public ActionResult CouponCreate(int? SellerId)
        {
            int? sellerId = Session["SellerId"] as int?;
            if (sellerId == null)
            { return RedirectToAction("Login", "SellerLogin"); }
            else
            { sellerId = (int)Session["SellerId"]; }

            SellerId = sellerId;
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

            var Sellers = db.Sellers.Where(x=>x.SellerId== SellerId).Select(x=>x.StoreName).FirstOrDefault().ToString();

            ViewBag.StoreName = Sellers;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CouponCreate([Bind(Include = "CouponId,SellerId,CouponNumber,CouponName,CouponDetails,Quantity,ReceivedQuantity,CouponTerms,CouponType,Discount,Storewide,StartTime,ClaimDeadline,EndTime,EventStatus")] CouponVM vm)
        {
            int sellerId = 1;
                sellerId = db.Sellers.Where(s => s.StoreName == vm.Storewide).Select(s => s.SellerId).FirstOrDefault();

            var coupon = new Coupon
            {
                CouponId = vm.CouponId,
                SellerId = sellerId,
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
                return RedirectToAction("CouponList");
            }

            return View(coupon);
        }

        public ActionResult MyPointList(int? SellerId)
        {
            int? sellerId = Session["SellerId"] as int?;
            if(sellerId == null) 
            { return RedirectToAction("Login", "SellerLogin");}
            else
            { sellerId =(int) Session["SellerId"]; }

            SellerId = sellerId;

            var point = new ADPoint();
            var model = ADPointChange.BuyPoint(point, (int)SellerId);
            ViewBag.List = db.ADPoints.Where(s => s.SellerId == SellerId).OrderByDescending(d => d.PurchaseTime).ToList();
            ViewBag.SellerPoint = db.Sellers.Where(s => s.SellerId == SellerId).Select(p => p.ADPoints).FirstOrDefault();
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyPointList([Bind(Include = "ADPointId,SellerId,GUINumber,PurchaseTime,ADPoints,PaymentStatus")] ADPointVM vm)
        {

            var sellerToUpdate = db.Sellers.FirstOrDefault(x => x.SellerId == vm.SellerId);
            if (sellerToUpdate != null)
            {
                sellerToUpdate.ADPoints = sellerToUpdate.ADPoints + vm.ADPoints;
                db.Entry(sellerToUpdate).State = EntityState.Modified;
                db.ADPoints.Add(ADPointChange.VM2ADPoint(vm));
                db.SaveChanges();
                return RedirectToAction("MyPointList");
            }

            return View(vm);
        }

        public ActionResult MyPointBuy()
        {
            return View();
        }

        //-------------其他方法----------------
        //生優惠券號
        public string GetCouponNums(string year, string month, int count)
        {
            return $"C{year}{month}{count.ToString().PadLeft(2, '0')}";
        }
    }
}