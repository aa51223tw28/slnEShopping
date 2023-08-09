using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Helpers;
using prjEShopping.Models.Infra;
using System.Security.Principal;
using System.Threading.Tasks;
using GoogleAuthentication.Services;
namespace prjEShopping.Controllers
{
    public class SellerLoginController : Controller
    {
        private AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult Login()
        {
            var clientId = "428102972567-vrsafsab1n7en9tuu7k7lajf77dcmcgo.apps.googleusercontent.com";
            var url = "https://localhost:8080/User/RedirectGoogleLogin";
            var response = GoogleAuth.GetAuthUrl(clientId, url);
            ViewBag.response = response;
            return View();
        }
        

        [HttpPost]
        public ActionResult Login(SellerLoginVM vm)
        {
            //using(var db=new AppDbContext())
            //{
                var sellerDetail = db.Sellers.Where(x=>x.SellerAccount == vm.SellerAccount && x.SellerPassword == vm.SellerPassword).FirstOrDefault();
                if(sellerDetail == null)
                {
                    vm.LoginErrorMessage = "帳號或密碼有誤!!";
                ViewBag.ShowErrorModal = true;
                return View("Login", vm);
                }
            else if (sellerDetail.AccessRightId != 1)
            {
                vm.LoginErrorMessage = "權限不對，無法登入!!";
                ViewBag.ShowErrorModal = true;
                return View("Login", vm);
            }
            else
                {
                    Session["SellerId"] = sellerDetail.SellerId;
                    Session["StoreName"] = sellerDetail.StoreName;
                    return RedirectToAction("Index", "SellerMain");
                }
           // }

        }
        
        public ActionResult SellerPasswordForgot()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SellerPasswordForgot(string SellerAccount)
        {
            var seller = db.Sellers.FirstOrDefault(s => s.SellerAccount == SellerAccount);

            if (seller == null)
            {
                ModelState.AddModelError("", "帳號不存在或無效。");
                return View();
            }

            var urlHelper = new UrlHelper(this.ControllerContext.RequestContext);
            EmailVerifyUrl.SendEmailUrl(SellerAccount, urlHelper, "EmailVerify", "Sellers");

            ViewBag.Message = "郵件已發送，請檢查您的信箱！";

            return View();
        }

        public ActionResult EmailVerify(string token)
        {
            var seller = db.Sellers.FirstOrDefault(s => s.EmailCheck == token);

            if (seller != null)
            {
                return RedirectToAction("ResetPassword",new { seller = seller.SellerAccount });
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult ResetPassword(string seller)
        {
            ViewBag.Account = seller;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string Account, string newPassword)
        {
            var seller = db.Sellers.FirstOrDefault(s => s.SellerAccount == Account);

            if (seller != null)
            {
                HashPassword hashPassword = new HashPassword(db); // 假设 _db 是你的数据库上下文
                string salt;
                string hash = hashPassword.CreateHashPassword(newPassword, out salt);

                seller.SellerPassword = hash;
                seller.SellerPasswordSalt = salt;
                seller.EmailCheck = null; // 清空 token
                db.SaveChanges();

                ViewBag.SuccessMessage = "密碼已重設成功！請使用新密碼登入。";
                return View();
            }

            ViewBag.ErrorMessage = "重設密碼失敗，請重新操作。";
            return View();
        }

        private bool CheckEmailExists(string sellerAccount)
        {
            using (var dbContext = new AppDbContext()) // 替换为你的 DbContext
            {
                return dbContext.Sellers.Any(u => u.SellerAccount == sellerAccount);
            }
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "SellerLogin");
        }
    }
}
