using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static prjEShopping.Controllers.testproductsupportController;

namespace prjEShopping.Controllers
{
    public class UserSupportController : Controller
    {
        // GET: UserSupport
        public ActionResult UserCSList()
        {
            var db = new AppDbContext();
            var customerAccount = User.Identity.Name;

            //找userid
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();

            ViewBag.UserId = userid;
            var s = db.Supports.Where(x => x.UserId == userid).ToList();
            var model = SupportChange.Support2VM(s);
            return View(model);            
        }
    }
}