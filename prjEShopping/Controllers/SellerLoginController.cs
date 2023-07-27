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
                    Session["SellerId"] = vm.SellerId;
                    return RedirectToAction("Index", "SellerMain");
                }
            }

            //var db = new AppDbContext();
            //if (ModelState.IsValid==true)
            //{
            //    var seller = db.Sellers.FirstOrDefault(m => m.SellerAccount == vm.SellerAccount && m.SellerPassword == vm.SellerPassword);
            //    if (seller != null)
            //    {
            //        return View("~/Views/SellerMain/Index.cshtml");
            //    }
            //    else
            //    {
            //        ModelState.AddModelError("", "請輸入正確帳號或密碼");
            //    }
            //}
            //return View(vm);
        }
    }
}
