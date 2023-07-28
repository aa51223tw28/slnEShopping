using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class SellerUpdateController : Controller
    {
        // GET: SellerUpdate
        public ActionResult Edit()
        {
            int? id = (int)Session["SellerId"];
            if (id == null)
                return RedirectToAction("List");
            var db = new AppDbContext();
            
            var data = db.Sellers.FirstOrDefault(x => x.SellerId == id);
            if (data == null)
                return RedirectToAction("List");
            var viewModel = new SellerRegisterVM
            {
                GUINumber=data.GUINumber,
                SellerName = data.SellerName,
                StoreName = data.StoreName,
                SellerImagePath = data.SellerImagePath,
                Phone=data.Phone,
                SellerAccount=data.SellerAccount,
                 SellerPassword=data.SellerPassword,
                Address=data.Address,
                BankAccount=data.BankAccount,
                PaymentMethodId=data.PaymentMethodId,
                ShippingMethodId = data.ShippingMethodId,
                StoreIntro =data.StoreIntro
            };
            return View(viewModel);    
        }

        [HttpPost]
        public ActionResult Edit(SellerRegisterVM s)
        {
            var editId = (int)Session["SellerId"];
            var db = new AppDbContext();
            var data = db.Sellers.FirstOrDefault(x => x.SellerId == editId);
            if (data != null)
            {
                string password = Request.Form["SellerPassword"];
                if (password != data.SellerPassword)
                {
                    TempData["Fail"] = "密碼輸入不正確！";
                    return View(s);
                }
                if (s.photo != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    data.SellerImagePath = photoName;
                    s.photo.SaveAs(Server.MapPath("../img/" + photoName));
                }
                data.SellerName = s.SellerName;
                data.Phone = s.Phone;
                data.SellerAccount = s.SellerAccount;
                data.SellerPassword = s.SellerPassword;
                data.Address = s.Address;
                data.BankAccount = s.BankAccount;
                data.StoreIntro = s.StoreIntro;
                data.PaymentMethodId = s.PaymentMethodId;
                data.ShippingMethodId = s.ShippingMethodId;
                //data.PaymentMethodId = (db.PaymentMethods.Where(x => x.PaymentMethodId == s.PaymentMethodId).SingleOrDefault()).PaymentMethodId;
                //data.ShippingMethodId = (db.ShippingMethods.Where(x => x.ShippingMethodId == s.ShippingMethodId).SingleOrDefault()).ShippingMethodId;
                db.SaveChanges();
                TempData["Success"] = "您已修改完成！";
                return RedirectToAction("Index", "SellerMain");
            } 
            return View("~/Views/SellerMain/Index.cshtml");
        }
    }
}