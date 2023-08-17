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
using System.Windows;

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
        

        public ActionResult SellerRegisterEmail(string token)
        {
            var db = new AppDbContext();
            var sellertoken = db.Sellers.FirstOrDefault(x => x.EmailCheck == token);
            if (sellertoken != null)
            {
                sellertoken.AccessRightId = 1;
                sellertoken.EmailCheck = null;
                db.SaveChanges();

                TempData["SuccessMessage"] = "您的郵箱已成功驗證";
            }
            else
            {
                TempData["ErrorMessage"] = "驗證已失敗";
            }
            return View();
        }


        public ActionResult ResetPassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ResetPassword(SellerPasswordChangeVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            //驗證舊密碼邏輯
            var customerAccount = User.Identity.Name;
            if (!ValidateOldPassword(customerAccount, vm.OldPassword))
            {
                ModelState.AddModelError("", "舊密碼輸入錯誤");
                TempData["ErrorMessage"] = "舊密碼輸入錯誤"; // 将错误消息保存到 TempData
                return View(vm);
            }

            //更新密碼邏輯
            if (!UpdatePassword(customerAccount, vm.NewPassword))
            {
                ModelState.AddModelError("", "密碼格式錯誤，新密碼需至少包含一個英文大寫，一個英文小寫，一個數字，一個符號，且至少8個字元");
                TempData["ErrorMessage"] = "密碼格式錯誤，新密碼需至少包含一個英文大寫，一個英文小寫，一個數字，一個符號，且至少8個字元"; // 将错误消息保存到 TempData
                return View(vm);
            }

            //發送驗證郵件
            SendVerificationEmailForgot(customerAccount);
            TempData["SuccessMessage"] = "驗證郵件已發送，請查收並完成驗證，5秒後自動登出頁面";
            return View(vm);
            //return RedirectToAction("UserLogin");
        }

        private bool ValidateOldPassword(string selleraccount, string oldPassword)//驗證舊密碼邏輯
        {
            var db = new AppDbContext();
            var seller = db.Sellers.FirstOrDefault(x => x.SellerAccount == selleraccount);
            var sellerpw= db.Sellers.Where(x => x.SellerAccount == selleraccount).Select(x => x.SellerPassword).FirstOrDefault();


            if (sellerpw == oldPassword)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private bool UpdatePassword(string selleraccount, string newPassword)//更新密碼邏輯
        {
            if (IsPasswordValid(newPassword))
            {
                var db = new AppDbContext();
                var sellernewPassword = db.Sellers.FirstOrDefault(x => x.SellerAccount == selleraccount);
                sellernewPassword.SellerPassword = newPassword;
                sellernewPassword.AccessRightId = 2;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }
        private bool IsPasswordValid(string password)
        {
            //密碼長度至少為8個字符
            if (password.Length < 8)
            {
                return false;
            }

            //一個英文大寫 一個英文小寫 一個數字 一個符號
            bool hasUpperCase = false;
            bool hasLowerCase = false;
            bool hasDigit = false;
            bool hasSymbol = false;
            foreach (char c in password)
            {
                if (char.IsUpper(c))
                {
                    hasUpperCase = true;
                }
                else if (char.IsLower(c))
                {
                    hasLowerCase = true;
                }
                else if (char.IsDigit(c))//檢查數字
                {
                    hasDigit = true;
                }
                else if (char.IsSymbol(c) || char.IsPunctuation(c))//檢查符號
                {
                    hasSymbol = true;
                }

            }
            return hasUpperCase && hasLowerCase && hasDigit && hasSymbol;
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
            SendVerificationEmailForgot(seller.SellerAccount);

            ViewBag.Message = "郵件已發送，請檢查您的信箱！";

            return View();
        }
        private void SendVerificationEmailForgot(string sellerAccount)//發送驗證郵件
        {
            //生成驗證連結
            var db = new AppDbContext();
            var selleraccount = db.Sellers.FirstOrDefault(x => x.SellerAccount == sellerAccount);
            if (selleraccount != null)
            {
                string verificationToken = Guid.NewGuid().ToString();
                selleraccount.EmailCheck = verificationToken;
                db.SaveChanges();

                string relativeUrl = Url.Action("NewPassword", "SellerLogin", new { token = verificationToken });
                string absoluteUrl = Request.Url.Scheme + "://" + Request.Url.Authority + relativeUrl;


                var fromAddress = new MailAddress("Eshopping17go@gmail.com", "E起購");
                var toAddress = new MailAddress(sellerAccount);
                string subject = "E起購忘記密碼驗證信";
                string body = $"<html><body><h3>請點擊以下連結驗證您的電子郵件:<a href=\"{absoluteUrl}\">驗證</a></h3></body></html>";

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("Eshopping17go@gmail.com", "ayakelsjzapfbtil"),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true//可以吃到html標籤
                })
                {
                    smtpClient.Send(message);
                }
            }
        }
        public ActionResult SellerVerifyEmail(string token)
        {
            var db = new AppDbContext();
            var sellertoken = db.Sellers.FirstOrDefault(x => x.EmailCheck == token);
            if (sellertoken != null)
            {
                sellertoken.AccessRightId = 1;
                sellertoken.EmailCheck = null;
                db.SaveChanges();

                TempData["SuccessMessage"] = "您的郵箱已成功驗證";
            }
            else
            {
                TempData["ErrorMessage"] = "驗證已失敗";
            }
            return View();
        }

        public ActionResult NewPassword(string account)
        {
            ViewBag.Account = account;
            return View();
        }

        [HttpPost]
        public ActionResult NewPassword(string Account, string newPassword)
        {
            var seller = db.Sellers.FirstOrDefault(a => a.SellerAccount == Account);
            if (seller != null)
            {
                seller.SellerPassword = newPassword;
                db.SaveChanges();

                ViewBag.SuccessMessage = "儲存成功！即將在三秒後跳轉到登入頁面...";
                return View();
            }

            ViewBag.ErrorMessage = "存取失敗，請重新操作。";
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "SellerLogin");
        }

    }
}
