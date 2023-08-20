using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class PowerBIController : Controller
    {
        // GET: PowerBI
        public ActionResult Index()
        {
            HttpCookie authCookie = Request.Cookies["AdminLogin"];
            if (authCookie == null || authCookie.Values["status"] != "AdminLogin" || authCookie.Values["AccessRightId"] != "1")
            {
                return RedirectToAction("Login","Admins");
            }
            string decodedName = HttpUtility.UrlDecode(authCookie.Values["userName"]);
            ViewBag.AdminName = decodedName;

            return View();
        }

        public ActionResult SellerIndex()
        {
            int? sellerId = Session["SellerId"] as int?;
            if (sellerId == null)
            { return RedirectToAction("Login", "SellerLogin"); }
            else
            { sellerId = (int)Session["SellerId"]; }
            ViewBag.SellerId = sellerId;
            return View();
        }
    }
}