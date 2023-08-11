using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class Support2Controller : Controller
    {
        AppDbContext db = new AppDbContext();
        // GET: Support2
        // CSS客服端
        // CS 客戶端
        public ActionResult CSSList()
        {
            var model = SupportChange.Support2VM(db.Supports);
            
            return View(model);
        }
    }
}