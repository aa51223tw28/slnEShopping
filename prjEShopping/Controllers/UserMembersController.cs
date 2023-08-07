using prjEShopping.Models.EFModels;
using prjEShopping.Models.Infra;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

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
            if (ModelState.IsValid == false) return View(vm);

            // 驗證帳密的正確性
            UserResult result = ValidLogin(vm);

            if (result.IsSuccess != true) // 若驗證失敗...
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View(vm);
            }

            const bool rememberMe = false; // 是否記住登入成功的會員

            // 若登入帳密正確,就開始處理後續登入作業,將登入帳號編碼之後,加到 cookie裡
            (string returnUrl, HttpCookie cookie) processResult = ProcessLogin(vm.UserAccount, rememberMe);
            
            Response.Cookies.Add(processResult.cookie);
            //return Redirect(processResult.returnUrl);
            
            // 根據使用者角色導向適當的頁面
            var db = new AppDbContext();
            var member = db.Users.FirstOrDefault(m => m.UserAccount == vm.UserAccount);
            var roles = member.Role.ToString();
            
            if (roles.Contains("Admin"))
            {
                return Redirect(ConfigurationManager.AppSettings["AdminRoleRedirectUrl"]);
            }
            else if (roles.Contains("User"))
            {
                return Redirect(ConfigurationManager.AppSettings["UserRoleRedirectUrl"]);
            }
            else if (roles.Contains("Seller"))
            {
                return Redirect(ConfigurationManager.AppSettings["SellerRoleRedirectUrl"]);
            }
            else
            {
                // 如果使用者角色不是 "Admin" 或 "User"，可以導向預設頁面
                return RedirectToAction("Default");
            }

        }

        private UserResult ValidLogin(UserLoginVM vm)
        {
            var db = new AppDbContext();
            var member = db.Users.FirstOrDefault(m => m.UserAccount == vm.UserAccount);

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

        private (string returnUrl, HttpCookie cookie) ProcessLogin(string account, bool rememberMe)
        {
            var roles = string.Empty; // 在本範例, 沒有用到角色權限,所以存入空白

            // 建立一張認證票
            var ticket =
                new FormsAuthenticationTicket(
                    1,          // 版本別, 沒特別用處
                    account,
                    DateTime.Now,   // 發行日
                    DateTime.Now.AddDays(2), // 到期日
                    rememberMe,     // 是否續存
                    roles,          // userdata
                    "/" // cookie位置
                );

            // 將它加密
            var value = FormsAuthentication.Encrypt(ticket);

            // 存入cookie
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, value);

            // 取得return url
            var url = FormsAuthentication.GetRedirectUrl(account, true); //第二個引數沒有用處

            return (url, cookie);
        }


        

        public ActionResult UserLogout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return Redirect("/Main/Index");
        }

        [Authorize]
        public ActionResult UserEditProfile()
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();

            var datas=db.Users.Where(x=>x.UserId==userid).Select(x=>new UserProfileVM()
            {
                UserName=x.UserName,
                UserAccount=x.UserAccount,
                CellPhone=x.CellPhone,
                Phone = x.Phone,
                City = x.City,
                Address = x.Address,
                Birthday= (DateTime)x.Birthday,
                Gender=x.Gender,
                UserImagePath=x.UserImagePath

            }).FirstOrDefault();

            return View(datas);
        }
        [Authorize]
        [HttpPost]
        public ActionResult UserEditProfile(UserProfileVM vm)
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();

            var data = db.Users.FirstOrDefault(x => x.UserId == userid);
            data.UserName = vm.UserName;
            data.UserAccount = vm.UserAccount;
            data.CellPhone = vm.CellPhone;
            data.Phone = vm.Phone;
            data.City = vm.City;
            data.Address = vm.Address;
            data.Birthday = vm.Birthday;
            data.Gender = vm.Gender;
            
            if(vm.UserImageFile != null&&vm.UserImageFile.ContentLength > 0)//上傳圖片
            {
                var fileName = Path.GetFileName(vm.UserImageFile.FileName);
                var imagePath = Path.Combine(Server.MapPath("~/img"), fileName);
                vm.UserImageFile.SaveAs(imagePath);

                data.UserImagePath = fileName; 
                
            }

            db.SaveChanges();

            return RedirectToAction("UserEditProfile");
        }
        [Authorize]
        public ActionResult UserEditPassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult UserEditPassword(UserChangePasswordVM vm)
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
            if(!UpdatePassword(customerAccount, vm.NewPassword))
            {
                ModelState.AddModelError("", "密碼格式錯誤，新密碼需至少包含一個英文大寫，一個英文小寫，一個數字，一個符號，且至少8個字元");
                TempData["ErrorMessage"] = "密碼格式錯誤，新密碼需至少包含一個英文大寫，一個英文小寫，一個數字，一個符號，且至少8個字元"; // 将错误消息保存到 TempData
                return View(vm);
            }

            //發送驗證郵件
            SendVerificationEmail(customerAccount);
            TempData["SuccessMessage"] = "驗證郵件已發送，請查收並完成驗證，5秒後自動登出頁面";
            return View(vm);
            //return RedirectToAction("UserLogin");
        }


        private bool ValidateOldPassword(string useraccount,string oldPassword)//驗證舊密碼邏輯
        {
            var db = new AppDbContext();

            var user = db.Users.FirstOrDefault(x => x.UserAccount == useraccount);
            var userpw = db.Users.Where(x=>x.UserAccount == useraccount).Select(x=>x.UserPassword).FirstOrDefault();
            
           
            if(userpw == oldPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
          
        }

        private bool UpdatePassword(string useraccount, string newPassword)//更新密碼邏輯
        {
            if (IsPasswordValid(newPassword))
            {
                var db = new AppDbContext();
                var usernewpw=db.Users.FirstOrDefault(x=>x.UserAccount==useraccount); 
                usernewpw.UserPassword = newPassword;
                usernewpw.AccessRightId = "2";
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
                if(char.IsUpper(c))
                {
                    hasUpperCase = true;
                }else if (char.IsLower(c))
                {
                    hasLowerCase = true;
                }else if (char.IsDigit(c))//檢查數字
                {
                    hasDigit = true;
                }
                else if (char.IsSymbol(c)||char.IsPunctuation(c))//檢查符號
                {
                    hasSymbol = true;
                }
                
            }
            return hasUpperCase && hasLowerCase && hasDigit&& hasSymbol;
        }

        private void SendVerificationEmail(string userAccount)//發送驗證郵件
        {
            //生成驗證連結
            var db=new AppDbContext();
            var useraccount = db.Users.FirstOrDefault(x => x.UserAccount == userAccount);
            if (useraccount != null)
            {
                string verificationToken = Guid.NewGuid().ToString();
                useraccount.EmailCheck=verificationToken;
                db.SaveChanges();

                //string verificationLink = "https://localhost:44388/UserMembers/UserVerifyEmail?token=" + verificationToken;
                string relativeUrl = Url.Action("UserVerifyEmail", "UserMembers", new { token = verificationToken });
                string absoluteUrl = Request.Url.Scheme + "://" + Request.Url.Authority + relativeUrl;


                var fromAddress = new MailAddress("Eshopping17go@gmail.com", "E起購");
                var toAddress = new MailAddress(userAccount);
                string subject = "E起購修改密碼驗證信";
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
                
        public ActionResult UserVerifyEmail(string token)
        {
            var db=new AppDbContext();
            var usertoken=db.Users.FirstOrDefault(x=>x.EmailCheck==token);
            if (usertoken != null)
            {
                usertoken.AccessRightId = "1";
                usertoken.EmailCheck = null;
                db.SaveChanges();

                TempData["SuccessMessage"] = "您的郵箱已成功驗證";               
            }
            else
            {
                TempData["ErrorMessage"] = "驗證已失敗";               
            }
            return View();
        }


        [Authorize]
        public ActionResult UserPhotoName()//傳送api使用者圖片跟名字到layout
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();

            var userdata=db.Users.Where(x=>x.UserId == userid).Select(x=>new UserProfileVM
            {
                UserName=x.UserName,
                UserImagePath =x.UserImagePath,
            }).FirstOrDefault();

            return Json(userdata, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UserRegister()
        {

            return View();
        }
        [HttpPost]
        public ActionResult UserRegister(UserProfileVM vm)
        {
            if (!ModelState.IsValid)
            {
                // 驗證失敗，返回原始的註冊視圖並顯示錯誤訊息
                return View(vm);
            }

            var db = new AppDbContext();

            //檢查是否有相同的UserAccount
            if (db.Users.Any(x => x.UserAccount == vm.UserAccount))
            {
                ModelState.AddModelError("UserAccount", $"{vm.UserAccount}該使用者帳號已經被註冊過");
                return View(vm);
            }

            string fileName = "";
            if (vm.UserImageFile != null && vm.UserImageFile.ContentLength > 0)
            {
                fileName = Path.GetFileName(vm.UserImageFile.FileName);
                var imagePath = Path.Combine(Server.MapPath("~/img"), fileName);
                vm.UserImageFile.SaveAs(imagePath);
            }

            var data = new User()
            {
                UserName = vm.UserName,
                UserAccount = vm.UserAccount,
                UserPassword = vm.UserPassword,
                CellPhone = vm.CellPhone,
                Phone = vm.Phone,
                City = vm.City,
                Address = vm.Address,
                Gender = vm.Gender,
                Birthday = vm.Birthday,
                Role = "User",
                AccessRightId = "2",//審核中
                ShippingMethodId = "1",
                PaymenyMethodId = "1",    
                UserImagePath=fileName,
            };
         
            db.Users.Add(data);
            db.SaveChanges();

            SendRegisterEmail(vm.UserAccount);

            TempData["SuccessMessage"] = "驗證郵件已發送，請查收並完成驗證，5秒後導向登入頁面";
            return View(vm);
        }

        private void SendRegisterEmail(string userAccount)//註冊驗證信
        {
            var db=new AppDbContext();
            var useraccount=db.Users.FirstOrDefault(x => x.UserAccount == userAccount);
            if(useraccount != null)
            {
                string verificationToken=Guid.NewGuid().ToString();
                useraccount.EmailCheck=verificationToken;
                db.SaveChanges();

                string relativeUrl = Url.Action("UserRegisterEmail", "UserMembers", new { token = verificationToken });
                string absoluteUrl = Request.Url.Scheme + "://" + Request.Url.Authority + relativeUrl;

                var fromAddress=new MailAddress("Eshopping17go@gmail.com", "E起購");
                var toAddress = new MailAddress(userAccount);
                string subject = "E起購開通會員驗證信";
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


        public ActionResult UserRegisterEmail(string token)
        {
            var db = new AppDbContext();
            var usertoken = db.Users.FirstOrDefault(x => x.EmailCheck == token);
            if (usertoken != null)
            {
                usertoken.AccessRightId = "1";
                usertoken.EmailCheck = null;
                db.SaveChanges();

                TempData["SuccessMessage"] = "您的郵箱已成功驗證";
            }
            else
            {
                TempData["ErrorMessage"] = "驗證已失敗";
            }
            return View();
        }
    }

}