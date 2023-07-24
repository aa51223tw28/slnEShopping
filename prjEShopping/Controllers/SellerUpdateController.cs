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
            Seller data = db.Sellers.Where(x => x.SellerId == id).SingleOrDefault();
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(Seller s)
        {
            var db = new AppDbContext();
            Seller data = db.Sellers.Where(x => x.SellerId == s.SellerId).FirstOrDefault();
            if (data != null)
            {
                if (s.photo != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    data.SellerImagePath = photoName;
                    s.photo.SaveAs(Server.MapPath("../../Images/" + photoName));
                }

                data.SellerName = s.SellerName;
                data.Phone = s.Phone;
                data.StoreName = s.StoreName;
                data.SellerPassword = s.SellerPassword;
                data.Address = s.Address;
                data.BankAccount = s.BankAccount;
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}