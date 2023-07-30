using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
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
            if (authCookie == null || authCookie.Value != "AdminLogin")
            {
                return RedirectToAction("Login");
            }
            var model = db.Admins.Admin2VM();
            return View(model);
        }

        //登入設置
        public ActionResult Login()
        {          
            return View();
        }

        [HttpPost]
        public ActionResult Login(AdminLoginVM vm)
        {
            var account = db.Admins.FirstOrDefault(a => a.AdminAccount == vm.AdminAccount);

            if (account != null)
            {
                var password = db.Admins.Where(a => a.AdminAccount == vm.AdminAccount).Select(p => p.AdminPassword).FirstOrDefault();
                if (vm.AdminPassword == password)
                {
                        HttpCookie authCookie = new HttpCookie("AdminLogin");
                        authCookie.Value = "AdminLogin";
                        authCookie.Expires = DateTime.Now.AddHours(1); // 设置过期时间
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

        // GET: Admins/Details/5
        public ActionResult Details(int? id)
        {
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
            return View(model);
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            ViewBag.AdminNumber =NewAccountNumber();
            var admin =new Admin();
            AdminVM model = AdminChange.Admin2VM(admin);
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
            string hash = hashPassword.CreatHashPassword(vm.AdminPassword, out salt);

            // 更新 ViewModel 中的密碼和鹽字段
            vm.AdminPassword = hash;
            vm.AdminPasswordSalt = salt;

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
            return View(model);
        }

        // POST: Admins/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdminId,AdminNumber,PermissionsId,AdminAccount,AdminPassword,AdminPasswordSalt,Title,AdminName,Phone,JobStatus,DateOfHire")] AdminVM vm)
        {//todo 密碼修改還沒做
            if (ModelState.IsValid==false)
                return View(vm);

                db.Entry(AdminChange.VM2AdminEdit(vm)).State = EntityState.Modified;
                db.SaveChanges();
            return RedirectToAction("Index");
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
