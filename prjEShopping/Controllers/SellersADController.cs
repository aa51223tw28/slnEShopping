using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class SellersADController : Controller
    {
        private AppDbContext db = new AppDbContext();
        // GET: SellersAD
        public ActionResult List()
        {

            // 獲取所有在SellersAD中存在的ADProductId
            var existingADProductIds = db.SellersADs.Select(sa => sa.ADProductId).ToList();

            // 從ADProducts中獲取不在existingADProductIds中的產品
            var model = db.ADProducts
                         .Where(ap => !existingADProductIds.Contains(ap.ADProductId))
                         .ToList()
                         .ADProduct2VM(); // 假設你有一個從ADProduct列表轉換為VM的方法

            return View(model);
        }

        public ActionResult CheckDetail(int adproductId)
        {
            int sellerId = 1;
            var model = db.ADProducts.Where(a => a.ADProductId == adproductId).ADProduct2VM().FirstOrDefault();

            ViewBag.SellerId = sellerId;
            ViewBag.Products=db.Products.Where(s=>s.SellerId==sellerId).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult CheckDetail(ADProductVM vm, int SellerId)
        { //todo 上傳圖檔

            if (ModelState.IsValid)
            {
                var ad=db.ADProducts.Where(x=>x.ADProductId==vm.ADProductId).FirstOrDefault();
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
                        ADPoints = -vm.ADPoint,
                        GUINumber = db.Sellers.Where(x => x.SellerId == SellerId).FirstOrDefault().ToString(),
                        PaymentStatus = "1",
                        PurchaseTime = DateTime.Now,
                    };
                    db.ADPoints.Add(point);

                    db.Sellers.Where(x => x.SellerId == SellerId).Select(s => s.ADPoints ==s.ADPoints-vm.ADPoint);

                    db.SaveChanges();
                    return RedirectToAction("List");
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
        {
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

        public ActionResult ADList()
        {
            var sellerId = 1;

            var sellerAdProductIds = db.SellersADs
                           .Where(x => x.SellerId == sellerId)
                           .Select(x => x.ADProductId)
                           .ToList();

            var model = db.ADProducts
                               .Where(x => sellerAdProductIds.Contains(x.ADProductId))
                               .ADProduct2VM();

            return View(model);
        }

        public ActionResult ADDemo()
        {
            var model = db.Products.Where(x=>x.SellerId==1).ToList();
            
            var AD=db.SellersADs.Where(x=>x.SellerId==1).Select(x=>x.ADProductId).ToList() ;

            var PD=db.ADProducts.Where(x => AD.Contains(x.ADProductId)).Select(x=>x.ProductId).ToList();

            ViewBag.PD = PD;

            return View(model);
        }


    }
}