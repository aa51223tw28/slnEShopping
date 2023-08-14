using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static prjEShopping.Controllers.testproductsupportController;

namespace prjEShopping.Controllers
{
    public class UserSupportController : Controller
    {
        // GET: UserSupport
        public ActionResult UserCSList()
        {
            var db = new AppDbContext();
            var customerAccount = User.Identity.Name;

            //找userid
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();

            ViewBag.UserId = userid;
            var s = db.Supports.Where(x => x.UserId == userid).ToList();
            var model = SupportChange.Support2VM(s);
            return View(model);            
        }

        //這跟商品頁面的投訴按鈕是一樣的 只是導回頁面不一樣
        //客戶端寄信
        public ActionResult UserCSSendMail()
        {  //客服信頁面寄信
            var db = new AppDbContext();
            var customerAccount = User.Identity.Name;

            //找userid
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();

            ViewBag.UserId = userid;
            var model = new SupportVM();
            //編號生成
            ViewBag.SupportNum = GenerateSupportNumberU();
            return View(model);
        }
        [HttpPost]
        public ActionResult UserCSSendMail(SupportVM vm)
        {
            var db = new AppDbContext();
            if (vm != null)
            {   //存圖..
                imageAdd(vm);
                var s = new Support();
                s = SupportChange.VM2Support(vm);
                db.Supports.Add(s);
                db.SaveChanges();
                return RedirectToAction("UserCSList");
            }
            return View(vm);
        }
        public string GenerateSupportNumberU()
        {
            var db = new AppDbContext();
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

        //客戶端回覆、查看
        public ActionResult UserCSReplay(int? id)
        {
            var db = new AppDbContext();
            var customerAccount = User.Identity.Name;

            //找userid
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();

            ViewBag.UserId = userid;
            if (!id.HasValue)
            {
                // 返回錯誤或重定向，因為沒有指定ID
                return RedirectToAction("UserCSList");
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
        public ActionResult UserCSReplay(SupportReplayVM vm)
        {
            var db = new AppDbContext();
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
                return RedirectToAction("UserCSReplay");
            }
            return View(vm);
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


    }
}