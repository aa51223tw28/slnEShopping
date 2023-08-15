using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
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
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var sellerId = db.Sellers.Where(x => x.SellerAccount == customerAccount).Select(x => x.SellerId).FirstOrDefault();

            var datas = db.Sellers.Where(x => x.SellerId == sellerId).Select(x => new SellerUpdateVM()
            {
                SellerName = x.SellerName,
                StoreName = x.StoreName,
                SellerAccount = x.SellerAccount,
                SellerPassword = x.SellerPassword,
                Phone = x.Phone,
                Address = x.Address,
                GUINumber =x.GUINumber,
                StoreIntro = x.StoreIntro,
                BankAccount = x.BankAccount,
                ShippingMethodId = x.ShippingMethodId,
                PaymentMethodId = x.PaymentMethodId,
                SellerImagePath = x.SellerImagePath
                
            }).FirstOrDefault();

            return View(datas);


            //int? id = (int)Session["SellerId"];
            //if (id == null)
            //    return RedirectToAction("List");
            //var db = new AppDbContext();
            
            //var data = db.Sellers.FirstOrDefault(x => x.SellerId == id);
            //if (data == null)
            //    return RedirectToAction("List");
            //var viewModel = new SellerRegisterVM
            //{
            //    GUINumber=data.GUINumber,
            //    SellerName = data.SellerName,
            //    StoreName = data.StoreName,
            //    //SellerImagePath = data.SellerImagePath,
            //    Phone=data.Phone,
            //    SellerAccount=data.SellerAccount,
            //     SellerPassword=data.SellerPassword,
            //    Address=data.Address,
            //    BankAccount=data.BankAccount,
            //    PaymentMethodId=data.PaymentMethodId,
            //    ShippingMethodId = data.ShippingMethodId,
            //    StoreIntro =data.StoreIntro
            //};
            //return View(viewModel);    
        }
        [Authorize]
        [HttpPost]
        public ActionResult Edit(SellerUpdateVM vm)
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var sellerId = db.Sellers.Where(x => x.SellerAccount == customerAccount).Select(x => x.SellerId).FirstOrDefault();

            var data = db.Sellers.FirstOrDefault(x => x.SellerId == sellerId);
            data.SellerName = vm.SellerName;
            data.StoreName = vm.StoreName;
            data.Phone = vm.Phone;
            data.SellerAccount = vm.SellerAccount;
            data.Address = vm.Address;
            data.GUINumber = vm.GUINumber;
            data.BankAccount = vm.BankAccount;
            data.PaymentMethodId = vm.PaymentMethodId;
            data.ShippingMethodId = vm.ShippingMethodId;
            data.StoreIntro = vm.StoreIntro;
            
            if (vm.SellerImageFile != null && vm.SellerImageFile.ContentLength > 0)//上傳圖片
            {
                var fileName = Path.GetFileName(vm.SellerImageFile.FileName);
                var imagePath = Path.Combine(Server.MapPath("~/img"), fileName);
                vm.SellerImageFile.SaveAs(imagePath);

                data.SellerImagePath = fileName;

            }

            db.SaveChanges();

            return RedirectToAction("Edit");
            //var editId = (int)Session["SellerId"];
            //var db = new AppDbContext();
            //var data = db.Sellers.FirstOrDefault(x => x.SellerId == editId);
            //if (data != null)
            //{
            //    string password = Request.Form["SellerPassword"];
            //    if (password != data.SellerPassword)
            //    {
            //        TempData["Fail"] = "密碼輸入不正確！";
            //        return View(s);
            //    }
            //    if (s.photo != null)
            //    {
            //        string photoName = Guid.NewGuid().ToString() + ".jpg";
            //        data.SellerImagePath = photoName;
            //        s.photo.SaveAs(Server.MapPath("../img/" + photoName));
            //    }
            //    data.SellerName = s.SellerName;
            //    data.Phone = s.Phone;
            //    data.SellerAccount = s.SellerAccount;
            //    data.SellerPassword = s.SellerPassword;
            //    data.Address = s.Address;
            //    data.BankAccount = s.BankAccount;
            //    data.StoreIntro = s.StoreIntro;
            //    data.PaymentMethodId = s.PaymentMethodId;
            //    data.ShippingMethodId = s.ShippingMethodId;
            //    db.SaveChanges();
            //    TempData["Success"] = "您已修改完成！";
            //    return RedirectToAction("Index", "SellerMain");
            //} 
            //return View("~/Views/SellerMain/Index.cshtml");
        }
    }
}