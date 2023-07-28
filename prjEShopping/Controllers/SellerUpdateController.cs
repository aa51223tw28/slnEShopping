using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class SellerUpdateController : Controller
    {
        // GET: SellerUpdate
        //public ActionResult List()
        //{
        //    var db = new AppDbContext();
        //    Seller sellers = new Seller();
        //    int id = (int)Session["SellerId"];
        //    sellers = db.Sellers.Where(x =>x.SellerId == id).FirstOrDefault();
        //    return View(sellers);
        //}
        public ActionResult Edit()
        {
            int? id = (int)Session["SellerId"];
            if (id == null)
                return RedirectToAction("List");
            var db = new AppDbContext();
            
            Seller data = db.Sellers.FirstOrDefault(x => x.SellerId == id);
            return View(data);    
        }

        [HttpPost]
        public ActionResult Edit(Seller s)
        {
            var editId = (int)Session["SellerId"];
            //Debug.WriteLine("editId: " + editId);
            var db = new AppDbContext();
            Seller data = db.Sellers.FirstOrDefault(x => x.SellerId == editId);
            if (data != null)
            {
                data.SellerName = s.SellerName;
                data.Phone = s.Phone;
                data.SellerPassword = s.SellerPassword;
                data.Address = s.Address;
                data.BankAccount = s.BankAccount;
                data.StoreIntro = s.StoreIntro;
                //data.PaymentMethodId = (db.PaymentMethods.Where(x => x.PaymentMethodId == s.PaymentMethodId).SingleOrDefault()).PaymentMethodId;
                //data.ShippingMethodId = (db.ShippingMethods.Where(x => x.ShippingMethodId == s.ShippingMethodId).SingleOrDefault()).ShippingMethodId;
                db.SaveChanges();
                TempData["Success"] = "您已修改完成！";
            } 
            return View("~/Views/SellerMain/Index.cshtml");
        }
    }
}