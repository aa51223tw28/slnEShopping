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
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(SellerLoginVM vm)
        {
            using(var db=new AppDbContext())
            {
                var sellerDetail = db.Sellers.Where(x=>x.SellerAccount == vm.SellerAccount && x.SellerPassword == vm.SellerPassword).FirstOrDefault();
                if(sellerDetail == null)
                {
                    vm.LoginErrorMessage = "帳號或密碼有誤!!";
                    return View("Login", vm);
                }
                else
                {
                    Session["StoreName"] = sellerDetail.StoreName;
                    return RedirectToAction("Index", "SellerMain");
                }
            }

        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "SellerLogin");
        }
    }
}
