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
            string t01 = "分支01";
            string t02 = "分支02修改";
            string t03 = "03修改"; //"03我也修改!";
            var db= new AppDbContext();
            List<Admin> admins = db.Admins.ToList();
            return View(admins);
        }

    }
}