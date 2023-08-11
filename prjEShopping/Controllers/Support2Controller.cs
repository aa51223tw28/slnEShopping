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
            //todo 分類頁面 新進信件/已回覆 Admin權限設定
            var model = SupportChange.Support2VM(db.Supports);
            
            return View(model);
        }

        //客服端寄信
        public ActionResult CSSSendMail(int? SellerId=null,int? UserId=null)
        {
            var AdminId=1; //要從帳號代入
            //UserId = 1;
            var model =new SupportVM();
            ViewBag.AdminId =AdminId;
            ViewBag.SellerId = SellerId;
            ViewBag.UserId = UserId;
            //編號生成
            ViewBag.SupportNum=GenerateSupportNumber();
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
        public ActionResult CSSReplay()
        {   //8/12 先做收信回覆框架 後面就可以套用回來
            return View();
        }

        //客戶端信件列表
        public ActionResult CSList()
        {
            return View();
        }

        //客戶端寄信
        public ActionResult CSSendMail()
        {  //客服信頁面寄信
            return View();
        }

        //客戶端回覆、查看
        public ActionResult CSReplay()
        {
            return View();
        }

        //客戶端檢舉信
        public ActionResult CSReport()
        { //Btn->彈出視窗寄信
            return View();
        }

        //商家端列表
        public ActionResult CSBList()
        {
            return View();
        }

        //商家端寄信
        public ActionResult CSBSendMail()
        {
            return View();
        }

        //商家端回覆
        public ActionResult CSBReplay()
        {
            return View();
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
    }
}