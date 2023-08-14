using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class MainController : Controller
    {
        // GET: UserMain
        public ActionResult Index()
        {
            //--Admin coolie 登入紀錄---
            HttpCookie authCookie = Request.Cookies["AdminLogin"];
            if (authCookie == null || authCookie.Values["status"] != "AdminLogin" || authCookie.Values["AccessRightId"] != "1")
            {
                return View();
            }
            string decodedName = HttpUtility.UrlDecode(authCookie.Values["userName"]);
            ViewBag.AdminName = decodedName;
            //---Admin coolie 登入紀錄---

            return View();
        }

        public ActionResult SearchSubcategoryIdapi(string subcategoryName)
        {
            var db = new AppDbContext();
            var subcategoryId = db.ProductSubCategories.Where(x => x.SubcategoryName == subcategoryName).Select(x=>x.SubcategoryId).FirstOrDefault();
            return Json(subcategoryId, JsonRequestBehavior.AllowGet);
        }
    }
}