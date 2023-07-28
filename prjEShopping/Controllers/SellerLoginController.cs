using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
                    Session["SellerId"] = sellerDetail.SellerId;
                    Session["StoreName"] = sellerDetail.StoreName;
                    return RedirectToAction("Index", "SellerMain");
                }
            }

        }
        //private string HashPassword(string password,string salt)
        //{
        //    using (SHA256 sha256 = SHA256.Create())
        //    {
        //        byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
        //        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        //        byte[] combinedBytes = new byte[saltBytes.Length + passwordBytes.Length];

        //        // Concatenate the salt and password bytes
        //        Buffer.BlockCopy(saltBytes, 0, combinedBytes, 0, saltBytes.Length);
        //        Buffer.BlockCopy(passwordBytes, 0, combinedBytes, saltBytes.Length, passwordBytes.Length);

        //        byte[] hashedBytes = sha256.ComputeHash(combinedBytes);

        //        // Convert the hashed bytes to a hexadecimal string
        //        StringBuilder builder = new StringBuilder();
        //        foreach (byte b in hashedBytes)
        //        {
        //            builder.Append(b.ToString("x2"));
        //        }
        //        return builder.ToString();
        //    }
        //}
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "SellerLogin");
        }
    }
}
