using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class SellerUpdateController : Controller
    {
        // GET: SellerUpdate
        public ActionResult List()
        {
            var db = new AppDbContext();
            Seller sellers = new Seller();
            sellers = db.Sellers.Where(x =>x.SellerId ==1).FirstOrDefault();
            return View(sellers);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            var db = new AppDbContext();
            Seller data = db.Sellers.FirstOrDefault(x => x.SellerId == id);
            return View(data);    
        }

        [HttpPost]
        public ActionResult Edit(Seller s)
        {
            var db = new AppDbContext();
            Seller data = db.Sellers.FirstOrDefault(x => x.SellerId == s.SellerId);
            if (data != null)
            {
                data.SellerName = s.SellerName;
                data.Phone = s.Phone;
                data.StoreName = s.StoreName;
                data.SellerPassword = s.SellerPassword;
                data.Address = s.Address;
                data.BankAccount = s.BankAccount;
                db.SaveChanges();
                TempData["UpdateSuccess"] = "您已修改完成！";
            } 
            return View("~/Views/SellerMain/Index.cshtml");
        }
    }
}