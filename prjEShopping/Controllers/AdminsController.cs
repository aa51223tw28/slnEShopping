using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using prjEShopping.Models.EFModels;
using prjEShopping.Models.Infra;
using prjEShopping.Models.ViewModels;

namespace prjEShopping.Controllers
{
    public class AdminsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Admins
        public ActionResult Index()
        {
            HttpCookie authCookie = Request.Cookies["AdminLogin"];
            if (authCookie == null || authCookie.Values["status"] != "AdminLogin" || authCookie.Values["AccessRightId"] != "1")
            {
                return RedirectToAction("Login");
            }
            string decodedName = HttpUtility.UrlDecode(authCookie.Values["userName"]);
            ViewBag.AdminName = decodedName;

            var model = db.Admins.Admin2VM();
            return View(model);
        }

        public ActionResult Indexem()
        {
            HttpCookie authCookie = Request.Cookies["AdminLogin"];
            if (authCookie == null || authCookie.Values["status"] != "AdminLogin" || authCookie.Values["AccessRightId"]!="1")
            {                
                return RedirectToAction("Login");
            }
            var model = db.Admins.Admin2VM().FirstOrDefault(a => a.AdminId == Convert.ToInt32(authCookie.Values["userId"]));        
                string decodedName = HttpUtility.UrlDecode(authCookie.Values["userName"]);
                ViewBag.AdminName = decodedName;           
            return View(model);
        }

        //登入設置
        public ActionResult Login()
        {
            HttpCookie authCookie = Request.Cookies["AdminLogin"];
            if (authCookie != null && authCookie.Values["status"] == "AdminLogin" &&authCookie.Values["AccessRightId"] == "1")
            {
                return RedirectToAction("Index");
            }
            else if (authCookie != null && authCookie.Values["AccessRightId"] != "1")
            {
                // 添加錯誤提示
                ModelState.AddModelError("", "帳號不存在或無效。");

                // 清除cookie
                authCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(authCookie);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(AdminLoginVM vm)
        {
            var account = db.Admins.FirstOrDefault(a => a.AdminAccount == vm.AdminAccount);

            if (account != null)
            {
                string storedHash = account.AdminPassword; // 從數據庫中取得已存儲的雜湊
                string storedSalt = account.AdminPasswordSalt; // 從數據庫中取得已存儲的鹽

                // 使用存儲的鹽和使用者提供的密碼來生成雜湊
                bool isVerified = HashPassword.VerifyPassword(vm.AdminPassword, storedSalt, storedHash);

                if (isVerified)
                {
                    HttpCookie authCookie = new HttpCookie("AdminLogin");
                    authCookie.Values["status"] = "AdminLogin";
                    authCookie.Values["AccessRightId"] = account.AccessRightId.ToString(); //權限=1才開通
                    authCookie.Values["userId"] = account.AdminId.ToString(); // 將用戶ID存儲在Cookie中
                    authCookie.Values["permissionsId"] = account.PermissionsId.ToString();
                    string encodedName = HttpUtility.UrlEncode(account.AdminName);
                    authCookie.Values["userName"] = encodedName;
                    authCookie.Expires = DateTime.Now.AddHours(1); // 設置過期時間
                    Response.Cookies.Add(authCookie);
                
                    return RedirectToAction("Index");


                }
            }

            ModelState.AddModelError(string.Empty, "帳號或密碼錯誤，請輸入正確數據!");
            return View("Login");
        }


        //登出設置
        public ActionResult Logout()
        {
            if (Request.Cookies["AdminLogin"] != null)
            {
                var c = new HttpCookie("AdminLogin");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
            return RedirectToAction("Login");
        }

        public ActionResult PWForgot()
        {//輸入帳號信箱
            return View();
        }

        [HttpPost]
        public ActionResult PWForgot(string AdminAccount)
        {
           var account= db.Admins.FirstOrDefault(a => a.AdminAccount == AdminAccount);

            if (account == null)
            {
                ModelState.AddModelError("", "帳號不存在或無效。");
                return View();
            }

            var urlHelper = new UrlHelper(this.ControllerContext.RequestContext);
            EmailVerifyUrl.SendEmailUrl(AdminAccount, urlHelper, "EmailVerify", "Admins");

            ViewBag.Message = "郵件已發送，請檢查您的信箱！";

            return View();
        }

        public ActionResult EmailVerify(string token)
        {   // 在此查找資料庫中與此token相匹配的帳戶
            var account = db.Admins.FirstOrDefault(a => a.EmailCheck == token);

            if (account != null)
            {
                // 轉到成功頁面
                return RedirectToAction("EmailVT", new { account = account.AdminAccount });
            }
            else
            {
                // 如果未找到匹配的帳戶，轉到錯誤頁面
                return RedirectToAction("Login");
            }
        }

        public ActionResult EmailVT(string account)
        { 
            ViewBag.Account = account;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmailVT(string Account, string newPassword)
        {
            var Admin = db.Admins.FirstOrDefault(a => a.AdminAccount == Account);

            if (Admin != null)
            {
                HashPassword hashPassword = new HashPassword(db); // 假設 _db 是你的資料庫上下文
                string salt;
                string hash = hashPassword.CreateHashPassword(newPassword, out salt);

                // 如果找到了相匹配的帳戶，更新帳戶狀態
                Admin.AccessRightId = 1; //將身份設為"已驗證"
                Admin.EmailCheck = null; //清空token
                Admin.AdminPassword = hash;
                Admin.AdminPasswordSalt = salt;
                db.SaveChanges();

                ViewBag.SuccessMessage = "儲存成功！即將在三秒後跳轉到登入頁面...";
                return View();
            }

            ViewBag.ErrorMessage = "存取失敗，請重新操作。";
            return View();
        }

        //public ActionResult EmailVF()
        //{ //驗證錯誤頁面
        //  //跳回登入頁面
        //  //這頁可以不做 (?)
        //    return View();
        //}



        // GET: Admins/Details/5
        public ActionResult Details(int? id)
        {
            HttpCookie authCookie = Request.Cookies["AdminLogin"];
            if (authCookie == null || authCookie.Values["status"] != "AdminLogin" || authCookie.Values["AccessRightId"]!="1")
            {
                return RedirectToAction("Login");
            }

            if (authCookie.Values["permissionsId"] !="1")
                return RedirectToAction("Indexem");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            AdminVM model = AdminChange.Admin2VM(admin);
            string decodedName = HttpUtility.UrlDecode(authCookie.Values["userName"]);
            ViewBag.AdminName = decodedName;
            return View(model);
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            HttpCookie authCookie = Request.Cookies["AdminLogin"];
            if (authCookie == null || authCookie.Values["status"] != "AdminLogin" || authCookie.Values["AccessRightId"]!="1")
            {
                return RedirectToAction("Login");
            }

            if (authCookie.Values["permissionsId"] != "1")
                return RedirectToAction("Indexem");

            ViewBag.AdminNumber =NewAccountNumber();
            var admin =new Admin();
            AdminVM model = AdminChange.Admin2VM(admin);
            string decodedName = HttpUtility.UrlDecode(authCookie.Values["userName"]);
            ViewBag.AdminName = decodedName;
            return View(model);
        }

        private string NewAccountNumber()
        {
            List<string> adminNumbers = db.Admins.Select(a => a.AdminNumber).ToList();

            var date = DateTime.Now;
            string year = date.Year.ToString().Substring(2, 2);
            string month = date.Month.ToString().PadLeft(2, '0');

            int count = 1;
            string adminNumber = GenerateAdminNumber(year, month,count);

            while (adminNumbers.Contains(adminNumber))
            {
                count++;
                adminNumber = GenerateAdminNumber(year, month, count);
            }

            return adminNumber;
        }

        private string GenerateAdminNumber(string year, string month,int count)
        {
            return $"A{year}{month}{count.ToString().PadLeft(2, '0')}";
        }

        // POST: Admins/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdminId,AdminNumber,PermissionsId,AdminAccount,AdminPassword,AdminPasswordSalt,Title,AdminName,Phone,DateOfHire,JobStatus,Role")] AdminVM vm)
        {
            SameAccount(vm);

            if (!ModelState.IsValid)
                return View(vm);

            HashPassword hashPassword = new HashPassword(db); // 假設 _db 是你的資料庫上下文
            string salt;
            string hash = hashPassword.CreateHashPassword(vm.AdminPassword, out salt);

            // 更新 ViewModel 中的密碼和鹽字段
            vm.AdminPassword = hash;
            vm.AdminPasswordSalt = salt;
            vm.AccessRightId = 1;
            db.Admins.Add(AdminChange.VM2Admin(vm));
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //相同帳號驗證方法
        private void SameAccount(AdminVM vm)
        {
            var data = db.Admins.FirstOrDefault(a => a.AdminAccount == vm.AdminAccount);
            if (data != null)
            {
                ViewBag.AdminNumber = NewAccountNumber();
                ModelState.AddModelError(string.Empty, "這個信箱已註冊，請用其他信箱進行申請帳號!");
            }
        }

        // GET: Admins/Edit/5
        public ActionResult Edit(int? id)
        {
            HttpCookie authCookie = Request.Cookies["AdminLogin"];
            if (authCookie == null || authCookie.Values["status"] != "AdminLogin" || authCookie.Values["AccessRightId"]!="1")
            {
                return RedirectToAction("Login");
            }

            if (authCookie.Values["permissionsId"] != "1") 
                 id = Convert.ToInt32(authCookie.Values["userId"]);
 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            AdminVM model = AdminChange.Admin2VM(admin);
            string decodedName = HttpUtility.UrlDecode(authCookie.Values["userName"]);
            ViewBag.AdminName = decodedName;
            return View(model);
        }

        // POST: Admins/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdminId,AdminNumber,PermissionsId,AdminAccount,AdminPassword,AdminPasswordSalt,Title,AdminName,Phone,JobStatus,DateOfHire,Role,AccessRightId")] AdminVM vm)
        {
            if (ModelState.IsValid==false)
                return View(vm);

                db.Entry(AdminChange.VM2AdminEdit(vm)).State = EntityState.Modified;
                db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult PasswordChange(int? id)
        {
            HttpCookie authCookie = Request.Cookies["AdminLogin"];
            if (authCookie == null || authCookie.Values["status"] != "AdminLogin" || authCookie.Values["AccessRightId"]!="1")
            {
                return RedirectToAction("Login");
            }

            if (authCookie.Values["permissionsId"] != "1")
            {
                id = Convert.ToInt32(authCookie.Values["userId"]);
            }

            int userId = (int)id;

            if (id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.UserName = db.Admins.Where(a => a.AdminId == userId).Select(n => n.AdminName).FirstOrDefault();
            }

            PasswordChangeVM model = new PasswordChangeVM
            {
                UserId =userId
            };
            string decodedName = HttpUtility.UrlDecode(authCookie.Values["userName"]);
            ViewBag.AdminName = decodedName;
            return View(model);
        }

        [HttpPost]
        public ActionResult PasswordChange(PasswordChangeVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var userId = vm.UserId;
            var account = db.Admins.FirstOrDefault(a => a.AdminId == userId);

            if (account != null)
            {
                string storedHash = account.AdminPassword;
                string storedSalt = account.AdminPasswordSalt;

                bool isVerified = HashPassword.VerifyPassword(vm.OldPassword, storedSalt, storedHash);

                if (!isVerified)
                {
                    ModelState.AddModelError("OldPassword", "舊密碼不正確");
                    return View(vm);
                }

                HashPassword hashPassword = new HashPassword(db);
                string newSalt;
                string newHash = hashPassword.CreateHashPassword(vm.NewPassword, out newSalt);

                account.AdminPassword = newHash;
                account.AdminPasswordSalt = newSalt;

                db.SaveChanges();

                return RedirectToAction("Edit", new {id=userId});
            }

            return View(vm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
