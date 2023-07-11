using prjEShopping.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult List()
        {
           EShoppingEntities db= new EShoppingEntities();
            List<Admin> admins = db.Admins.ToList();
            return View(admins);
        }

    }
}