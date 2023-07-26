using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace prjEShopping.Controllers
{
    public class SellerLoginController : Controller
    {
        // GET: SellerLogin
        public ActionResult Login(SellerLoginVM vm)
        {
            var db = new AppDbContext();
            var member = db.Sellers.FirstOrDefault(m => m.SellerAccount == vm.SellerAccount);
            return View(/*"~/Views/SellerMain/Index.cshtml"*/);
        }
       
    }
}