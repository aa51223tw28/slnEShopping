using prjEShopping.Models;
using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class SellerRegisterController : Controller
    {
        public ActionResult List()
        {
            AppDbContext db = new AppDbContext();
            var datas = from t in db.Sellers
                        select t;
            return View(datas);
        }
        public ActionResult List1()
        {
            AppDbContext db = new AppDbContext();
            var datas = from t in db.Sellers
                        select t;
            return Json(datas);
        }
        // GET: SellerRegister
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Seller s)
        {
            var db = new AppDbContext();
            db.Sellers.Add(s);
            db.SaveChanges();
            return RedirectToAction("List");
            
        }


        //public ActionResult Save()
        //{
        //    SellerRegisterVM s = new SellerRegisterVM();
        //    s.GUINumber = Request.Form["txtGUINumber"];
        //    s.SellerName = Request.Form["txtSellerName"];
        //    s.StoreName = Request.Form["txtStoreName"];
        //    s.SellerAccount = Request.Form["txtSellerAccount"];
        //    s.SellerPassword = Request.Form["txtSellerPassword"];
        //    s.Address = Request.Form["txtAddress"];
        //    s.Phone = Request.Form["txtPhone"];
        //    s.BankAccount = Request.Form["txtBankAccount"];
        //    s.StoreIntro = Request.Form["txtStoreIntro"];
        //     (new CFactory()).create(s);
        //    return RedirectToAction("List");
        //}
    }
}