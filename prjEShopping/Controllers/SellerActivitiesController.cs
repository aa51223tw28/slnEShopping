using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class SellerActivitiesController : Controller
    {
        // GET: SellerActivities
        public ActionResult ADList()
        {
            return View();
        }

        public ActionResult ADCreate()
        {
            return View();
        }

        public ActionResult CouponList()
        {
            return View();
        }

        public ActionResult CouponCreate()
        {
            return View();
        }

        public ActionResult MyPointList()
        {
            return View();
        }

        public ActionResult MyPointBuy()
        {
            return View();
        }

    }
}