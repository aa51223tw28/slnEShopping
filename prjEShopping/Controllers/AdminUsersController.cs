using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;

namespace prjEShopping.Controllers
{
    public class AdminUsersController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: AdminUsers
        public ActionResult Index()
        {
            HttpCookie authCookie = Request.Cookies["AdminLogin"];
            if (authCookie == null || authCookie.Values["status"] != "AdminLogin" || authCookie.Values["AccessRightId"] != "1")
            {
                return RedirectToAction("Login", "Admins");
            }
            string decodedName = HttpUtility.UrlDecode(authCookie.Values["userName"]);
            ViewBag.AdminName = decodedName;

            var model = AdminUserChange.AdminUser2VM(db.Users);

            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeAccessRightId(int id, string AccessRightId)
        {
            var user = db.Users.Find(id);
            user.AccessRightId = AccessRightId;
            db.SaveChanges();

            // Return the new status
            return Json(new { newAccessRightId = AccessRightId });
        }

        // GET: AdminUsers/Details/5
        public ActionResult Details(int? id)
        {
            HttpCookie authCookie = Request.Cookies["AdminLogin"];
            if (authCookie == null || authCookie.Values["status"] != "AdminLogin" || authCookie.Values["AccessRightId"] != "1")
            {
                return RedirectToAction("Login", "Admins");
            }
            string decodedName = HttpUtility.UrlDecode(authCookie.Values["userName"]);
            ViewBag.AdminName = decodedName;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: AdminUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            HttpCookie authCookie = Request.Cookies["AdminLogin"];
            if (authCookie == null || authCookie.Values["status"] != "AdminLogin" || authCookie.Values["AccessRightId"] != "1")
            {
                return RedirectToAction("Login", "Admins");
            }
            string decodedName = HttpUtility.UrlDecode(authCookie.Values["userName"]);
            ViewBag.AdminName = decodedName;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            var model=AdminUserChange.AdminUser2VM(user);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: AdminUsers/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,UserName,UserAccount,UserPassword,UserPasswordSalt,Gender,Phone,CellPhone,City,Address,ShippingMethodId,PaymenyMethodId,Birthday,AccessRightId,UserImagePath,Role,EmailCheck")] AdminUserVM vm)
        {
            if (ModelState.IsValid)
            {
                db.Entry(AdminUserChange.VM2AdminUser(vm)).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
