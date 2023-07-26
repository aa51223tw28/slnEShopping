using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace prjEShopping.Controllers
{
    public class SellerLoginController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(SellerLoginVM vm)
        {
            var db = new AppDbContext();
            if (ModelState.IsValid)
            {
                var seller = db.Sellers.FirstOrDefault(m => m.SellerAccount == vm.SellerAccount && m.SellerPassword == vm.SellerPassword);
                if (seller != null)
                {
                    return View("~/Views/SellerMain/Index.cshtml");
                }
                else
                {
                    // 登入失敗，顯示錯誤訊息
                    ModelState.AddModelError("", "請輸入正確帳號或密碼");
                }
            }

            // 若登入失敗，或模型驗證失敗，返回登入頁面，並保留使用者輸入的資料
            return View(vm);
        }
    }
}
