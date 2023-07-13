using prjEShopping.Models;
using prjEShopping.Models.EFModels;
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
           var db= new AppDbContext();
            List<Admin> admins = db.Admins.ToList();
            return View(admins);
        }

    }
}