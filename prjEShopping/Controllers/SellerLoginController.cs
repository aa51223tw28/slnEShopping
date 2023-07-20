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
        // GET: SellerLogin
        public ActionResult Login()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult Login(string account,string password,string chkRemember)
        //{
        //    if (FormsAuthentication.Authenticate(account, password))
        //    {
        //        FormsAuthentication.RedirectFromLoginPage(account, chkRemember == "on");
        //    }
        //    else
        //    {
        //        ViewBag.jsCode = "<script>alert('您輸入的帳號密碼有誤，請重新輸入');</script>";
        //    }
        //    return View();
        //}
        //public ActionResult Logout()
        //{
        //    FormsAuthentication.SignOut();
        //    return Redirect("/SellerLogin/Login/");
        //}
    }
}