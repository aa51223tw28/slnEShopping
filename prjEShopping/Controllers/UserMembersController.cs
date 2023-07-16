using prjEShopping.Models.EFModels;
using prjEShopping.Models.Infra;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace prjEShopping.Controllers
{
    public class UserMembersController : Controller
    {
        // GET: Members
        public ActionResult UserLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserLogin(UserLoginVM vm)
        {
            if (ModelState.IsValid == false) return View(vm);//以便用戶能夠看到錯誤訊息和原始輸入值，並進行必要的修正

            //驗證帳密的正確性
            UserResult result = VailLogin(vm);

            if (result.IsSuccess != true)//若驗證失敗
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View();
            }
            return RedirectToAction("Index", "UserMain");
            //const bool remeberMe = false;//是否記住登入成功的會員
            ////若登入帳密正確,就會開始處理後續登入作業,將登入帳號編碼後,加到cookie裡
            //(string returnUrl, HttpCookie cookie) processResult = ProcessLogin(vm.UserAccount, remeberMe);

            //Response.Cookies.Add(processResult.cookie);
            //return Redirect(processResult.returnUrl);


        }

        private UserResult VailLogin(UserLoginVM vm)//判斷帳密是否能進入的方法
        {
            var db = new AppDbContext();
            var member = db.Users.FirstOrDefault(x => x.UserAccount == vm.UserAccount);//判斷帳號

            if (member == null) return UserResult.Fail("帳密有誤");

            //會員資格要是AccessRightId==1(使用中)
            var right = member.AccessRightId.ToString();

            if (right != "1")
            {
                return UserResult.Fail("會員資格尚未確認");
            }


            //待補密碼雜湊
            //var salt = HashUtilitiy.GetSalt();
            //var hashPassword=HashUtilitiy.ToSHA256(vm.UserPassword,salt);

            return member.UserPassword == vm.UserPassword ? UserResult.Success() : UserResult.Fail("帳密有誤");//判斷密碼
        }

        //private (string returnUrl, HttpCookie cookie) ProcessLogin(string account, bool rememberMe)
        //{
        //    var roles = string.Empty; // 在本範例, 沒有用到角色權限,所以存入空白

        //    // 建立一張認證票
        //    var ticket =
        //        new FormsAuthenticationTicket(
        //            1,          // 版本別, 沒特別用處
        //            account,
        //            DateTime.Now,   // 發行日
        //            DateTime.Now.AddDays(2), // 到期日
        //            rememberMe,     // 是否續存
        //            roles,          // userdata
        //            "/" // cookie位置
        //        );

        //    // 將它加密
        //    var value = FormsAuthentication.Encrypt(ticket);

        //    // 存入cookie
        //    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, value);

        //    // 取得return url
        //    var url = FormsAuthentication.GetRedirectUrl(account, true); //第二個引數沒有用處

        //    return (url, cookie);
        //}


        public ActionResult UserLogout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return Redirect("/Main/Index");
        }
    }

}