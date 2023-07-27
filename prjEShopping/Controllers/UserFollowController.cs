using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class UserFollowController : Controller
    {
        // GET: UserFollow
        [Authorize]
        public ActionResult UserFollowSeller()
        {
            return View();
        }


        [Authorize]
        public ActionResult UserFollowProduct()
        {
            return View();
        }
    }
}