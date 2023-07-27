using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class UserCouponController : Controller
    {
        // GET: UserCoupon
        [Authorize]
        public ActionResult UserCouponList()
        {
            return View();
        }
    }
}