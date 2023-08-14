using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace prjEShopping.Controllers
{
    public class Support2Controller : Controller
    {
        AppDbContext db = new AppDbContext();
        // GET: Support2
        // CSS 客服端
        // CS  客戶端
        // CSB 商家端

        //客服端信件列表
        public ActionResult CSSList()
        {
            HttpCookie authCookie = Request.Cookies["AdminLogin"];
            if (authCookie == null || authCookie.Values["status"] != "AdminLogin" || authCookie.Values["AccessRightId"] != "1")
            {
                return RedirectToAction("Login", "Admins");
            }
            string decodedName = HttpUtility.UrlDecode(authCookie.Values["userName"]);
            ViewBag.AdminName = decodedName;
            //todo 分類頁面 新進信件/已回覆 Admin權限設定
            var model = SupportChange.Support2VM(db.Supports);

            return View(model);
        }

        //客服端寄信
        public ActionResult CSSSendMail(int? SellerId = null, int? UserId = null)
        {
            HttpCookie authCookie = Request.Cookies["AdminLogin"];
            if (authCookie == null || authCookie.Values["status"] != "AdminLogin" || authCookie.Values["AccessRightId"] != "1")
            {
                return RedirectToAction("Login", "Admins");
            }
            string decodedName = HttpUtility.UrlDecode(authCookie.Values["userName"]);
            ViewBag.AdminName = decodedName;
            int AdminId = Convert.ToInt32(HttpUtility.UrlDecode(authCookie.Values["userId"]));

            //UserId = 1;
            var model = new SupportVM();
            ViewBag.AdminId = AdminId;
            ViewBag.SellerId = SellerId;
            ViewBag.UserId = UserId;
            //編號生成
            ViewBag.SupportNum = GenerateSupportNumber();
            return View(model);
        }

        [HttpPost]
        public ActionResult CSSSendMail(SupportVM vm)
        {
            if (vm != null)
            {   //存圖..
                imageAdd(vm);
                var s = new Support();
                s = SupportChange.VM2Support(vm);
                db.Supports.Add(s);
                db.SaveChanges();
                return RedirectToAction("CSSList");
            }

            return View(vm);
        }

        //客服端回覆、查看
        public ActionResult CSSReplay(int? id)
        {
            HttpCookie authCookie = Request.Cookies["AdminLogin"];
            if (authCookie == null || authCookie.Values["status"] != "AdminLogin" || authCookie.Values["AccessRightId"] != "1")
            {
                return RedirectToAction("Login", "Admins");
            }
            string decodedName = HttpUtility.UrlDecode(authCookie.Values["userName"]);
            ViewBag.AdminName = decodedName;

            var Adminid = 1;
            ViewBag.AdminId = Adminid;
            if (!id.HasValue)
            {
                // 返回錯誤或重定向，因為沒有指定ID
                return RedirectToAction("CSSList");
            }
            var s = db.Supports.FirstOrDefault(x => x.SupportId == id);
            if (s == null)
            {
                // 沒有找到首封信件
                return HttpNotFound();
            }

            var r = db.SupportReplaies.Where(x => x.SupportId == id).ToList();
            var model = new SupportDetailViewModel
            {
                Support = SupportChange.Support2VM(s),
                SupportReplies = r.Select(reply => SupportReplayChange.SupportReplay2VM(reply)).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult CSSReplay(SupportReplayVM vm)
        {
            if (vm != null)
            {   //存圖..
                imageAdd2(vm);
                var r = new SupportReplay();
                r = SupportReplayChange.VM2SupportReplay(vm);
                db.SupportReplaies.Add(r);
                var s = db.Supports.Find(vm.SupportId);
                if (s != null)
                {
                    s.SupportStatus = "已回覆";
                }
                db.SaveChanges();
                return RedirectToAction("CSSReplay");
            }
            return View(vm);
        }


        [HttpPost]
        public JsonResult UpdateStatus(int id)
        {
            var record = db.Supports.Find(id);
            if (record != null)
            {
                record.SupportStatus = "已完成"; // 對欄位進行更新
                db.SaveChanges();
                return Json(new { success = true, message = "更新成功!" });
            }
            return Json(new { success = false, message = "更新失敗!" });
        }

        //客戶端信件列表
        public ActionResult CSList(int? userId = null)
        {
            //userId
            userId = 1;
            ViewBag.UserId = userId;
            var s = db.Supports.Where(x => x.UserId == userId).ToList();
            var model = SupportChange.Support2VM(s);
            return View(model);
        }

        //客戶端寄信
        public ActionResult CSSendMail(int? UserId = null)
        {  //客服信頁面寄信
            UserId = 1;
            ViewBag.UserId = UserId;
            var model = new SupportVM();           
            //編號生成
            ViewBag.SupportNum = GenerateSupportNumberU();
            return View(model);
        }
        [HttpPost]
        public ActionResult CSSendMail(SupportVM vm)
        {
            if (vm != null)
            {   //存圖..
                imageAdd(vm);
                var s = new Support();
                s = SupportChange.VM2Support(vm);
                db.Supports.Add(s);
                db.SaveChanges();
                return RedirectToAction("CSList");
            }
            return View(vm);          
        }
        //客戶端回覆、查看
        public ActionResult CSReplay(int? id)
        {
            //帳號登入代入
            var UserId = 1;
            ViewBag.UserId = UserId;
            if (!id.HasValue)
            {
                // 返回錯誤或重定向，因為沒有指定ID
                return RedirectToAction("CSList");
            }
            var s = db.Supports.FirstOrDefault(x => x.SupportId == id);
            if (s == null)
            {
                // 沒有找到首封信件
                return HttpNotFound();
            }
            var r = db.SupportReplaies.Where(x => x.SupportId == id).ToList();
            var model = new SupportDetailViewModel
            {
                Support = SupportChange.Support2VM(s),
                SupportReplies = r.Select(reply => SupportReplayChange.SupportReplay2VM(reply)).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult CSReplay(SupportReplayVM vm)
        {
            if (vm != null)
            {   //存圖..
                imageAdd2(vm);
                var r = new SupportReplay();
                r = SupportReplayChange.VM2SupportReplay(vm);
                db.SupportReplaies.Add(r);
                var s = db.Supports.Find(vm.SupportId);
                if (s != null)
                {
                    s.SupportStatus = "待回覆";
                }
                db.SaveChanges();
                return RedirectToAction("CSReplay");
            }
            return View(vm);
        }

        //客戶端檢舉信
        public ActionResult CSReport(int? userId,int? productId)
        { //Btn->彈出視窗寄信
            userId = 1;
            productId = 1;
            var productName=db.Products.FirstOrDefault(x=>x.ProductId==productId).ProductName.ToString();
            var s = new Support();
            ViewBag.UserId = userId;
            ViewBag.ProductId=productId;
            ViewBag.ProductName =productName;
            ViewBag.Support = s;
            ViewBag.SupportNumber = GenerateSupportNumberU();
            return View();
        }

        //商家端列表
        public ActionResult CSBList(int? sellerId = null)
        {
            sellerId = 1;
            ViewBag.SellerId = sellerId;
            var s = db.Supports.Where(x => x.SellerId == sellerId).ToList();
            var model = SupportChange.Support2VM(s);
            return View(model);

        }

        //商家端寄信
        public ActionResult CSBSendMail(int? sellerId = null)
        {  //商家頁面寄信
            sellerId = 1;
            ViewBag.SellerId = sellerId;
            var model = new SupportVM();
            //編號生成
            ViewBag.SupportNum = GenerateSupportNumberS();
            return View(model);
        }

        [HttpPost]
        public ActionResult CSBSendMail(SupportVM vm)
        {
            if (vm != null)
            {   //存圖..
                imageAdd(vm);
                var s = new Support();
                s = SupportChange.VM2Support(vm);
                db.Supports.Add(s);
                db.SaveChanges();
                return RedirectToAction("CSBList");
            }
            return View(vm);
        }

        //商家端回覆
        public ActionResult CSBReplay(int? id)
        {
            //帳號登入代入
            var SellerId = 1;
            ViewBag.SellerId = SellerId;
            if (!id.HasValue)
            {
                // 返回錯誤或重定向，因為沒有指定ID
                return RedirectToAction("CSBList");
            }
            var s = db.Supports.FirstOrDefault(x => x.SupportId == id);
            if (s == null)
            {
                // 沒有找到首封信件
                return HttpNotFound();
            }
            var r = db.SupportReplaies.Where(x => x.SupportId == id).ToList();
            var model = new SupportDetailViewModel
            {
                Support = SupportChange.Support2VM(s),
                SupportReplies = r.Select(reply => SupportReplayChange.SupportReplay2VM(reply)).ToList()
            };
            return View(model);
        }
        
        [HttpPost]
        public ActionResult CSBReplay(SupportReplayVM vm)
        {
            if (vm != null)
            {   //存圖..
                imageAdd2(vm);
                var r = new SupportReplay();
                r = SupportReplayChange.VM2SupportReplay(vm);
                db.SupportReplaies.Add(r);
                var s = db.Supports.Find(vm.SupportId);
                if (s != null)
                {
                    s.SupportStatus = "待回覆";
                }
                db.SaveChanges();
                return RedirectToAction("CSBReplay");
            }
            return View(vm);
        }


        //------------------------------------以下都是方法-----------------------------------------
        //存圖範例
        private void imageAdd(SupportVM vm)
        {
            if (vm.ImageFile != null && vm.ImageFile.ContentLength > 0)
            {
                // 生成一個唯一的檔案名稱
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.ImageFile.FileName);

                // 指定檔案的保存路徑
                var path = Path.Combine(Server.MapPath("~/img/"), fileName);

                // 儲存檔案
                vm.ImageFile.SaveAs(path);

                // 將檔案名稱保存到Support模型的相應欄位
                vm.ImageLink = fileName;
            }
        }

        private void imageAdd2(SupportReplayVM vm)
        {
            if (vm.ImageFile != null && vm.ImageFile.ContentLength > 0)
            {
                // 生成一個唯一的檔案名稱
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.ImageFile.FileName);

                // 指定檔案的保存路徑
                var path = Path.Combine(Server.MapPath("~/img/"), fileName);

                // 儲存檔案
                vm.ImageFile.SaveAs(path);

                // 將檔案名稱保存到Support模型的相應欄位
                vm.ImageLink = fileName;
            }
        }

        //編號範例
        public string GenerateSupportNumber()
        {
            var today = DateTime.Now;
            // 獲取當前日期的格式化字符串 "230812"
            var datePart = today.ToString("yyMMdd");

            // 查詢當天已有多少條記錄
            var countForToday = db.Supports
                                  .Where(s => s.SupportNumber.StartsWith("CS" + datePart))
                                  .Count();
            // 生成下一個編號
            var numberPart = (countForToday + 1).ToString("D4"); // 四位數，不足前面補0

            return "CS" + datePart + numberPart;
        }

        //user 寄送
        public string GenerateSupportNumberU()
        {
            var today = DateTime.Now;
            // 獲取當前日期的格式化字符串 "230812"
            var datePart = today.ToString("yyMMdd");

            // 查詢當天已有多少條記錄
            var countForToday = db.Supports
                                  .Where(s => s.SupportNumber.StartsWith("US" + datePart))
                                  .Count();
            // 生成下一個編號
            var numberPart = (countForToday + 1).ToString("D4"); // 四位數，不足前面補0

            return "US" + datePart + numberPart;
        }

        //Seller 寄送
        public string GenerateSupportNumberS()
        {
            var today = DateTime.Now;
            // 獲取當前日期的格式化字符串 "230812"
            var datePart = today.ToString("yyMMdd");

            // 查詢當天已有多少條記錄
            var countForToday = db.Supports
                                  .Where(s => s.SupportNumber.StartsWith("SS" + datePart))
                                  .Count();
            // 生成下一個編號
            var numberPart = (countForToday + 1).ToString("D4"); // 四位數，不足前面補0

            return "SS" + datePart + numberPart;
        }
    }
}