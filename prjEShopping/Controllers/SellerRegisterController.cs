using prjEShopping.Models;
using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

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
        
        // GET: SellerRegister
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Create(SellerRegisterVM vm/* HttpPostedFileBase photo*/)
        {
            //if (!ModelState.IsValid)
            //{
            //    // 驗證失敗，返回原始的註冊視圖並顯示錯誤訊息
            //    return View(vm);
            //}
            var db = new AppDbContext();
            //檢查是否有相同的UserAccount
            var newAccount = db.Sellers.FirstOrDefault(x => x.SellerAccount == vm.SellerAccount);
            if (newAccount != null)
            {
                TempData["Fail"] = "此帳號已經被註冊！";
                return View(vm);
            }
            if (!IsStrongPassword(vm.SellerPassword))
            {
                TempData["Fail"] = "密碼需至少包含一個英文大寫、一個英文小寫、一個數字、一個符號，且至少8個字元！";
                return View(vm);
            }
            if (String.IsNullOrEmpty(vm.GUINumber))
            {
                TempData["Fail"] = "統編為必填欄位！";
                return View(vm);
            }
            if (String.IsNullOrEmpty(vm.SellerName))
            {
                TempData["Fail"] = "負責人姓名為必填欄位！";
                return View(vm);
            }
            if (String.IsNullOrEmpty(vm.StoreName))
            {
                TempData["Fail"] = "商家姓名為必填欄位！";
                return View(vm);
            }
            if (String.IsNullOrEmpty(vm.SellerAccount))
            {
                TempData["Fail"] = "電子郵件為必填欄位！";
                return View(vm);
            }
            if (String.IsNullOrEmpty(vm.SellerPassword))
            {
                TempData["Fail"] = "使用者密碼為必填欄位！";
                return View(vm);
            }
            if (String.IsNullOrEmpty(vm.Address))
            {
                TempData["Fail"] = "住址為必填欄位！";
                return View(vm);
            }
            if (String.IsNullOrEmpty(vm.Phone))
            {
                TempData["Fail"] = "手機號碼為必填欄位！";
                return View(vm);
            }
            if (String.IsNullOrEmpty(vm.BankAccount))
            {
                TempData["Fail"] = "銀行帳戶為必填欄位！";
                return View(vm);
            }
            if (String.IsNullOrEmpty(vm.StoreIntro))
            {
                TempData["Fail"] = "商城介紹為必填欄位！";
                return View(vm);
            }
            string fileName = "";
            if (vm.SellerImagePath != null && vm.SellerImagePath.ContentLength > 0)
            {
                fileName = Path.GetFileName(vm.SellerImagePath.FileName);
                var imagePath = Path.Combine(Server.MapPath("~/img"), fileName);
                vm.SellerImagePath.SaveAs(imagePath);
            }

            var newSeller = new Seller()
            {
                SellerName = vm.SellerName,
                StoreName= vm.StoreName,
                SellerAccount = vm.SellerAccount,
                SellerPassword = vm.SellerPassword,
                Phone = vm.Phone,
                Address = vm.Address,
                GUINumber = vm.GUINumber,
                StoreIntro = vm.StoreIntro,
                BankAccount = vm.BankAccount,
                AccessRightId = 2,//審核中
                ShippingMethodId = vm.ShippingMethodId,
                PaymentMethodId = vm.PaymentMethodId,
                SellerImagePath = fileName,
                Role = "Seller",
            };
            db.Sellers.Add(newSeller);
            db.SaveChanges();//先存進資料庫已讓有新的userId

            SendRegisterEmail(vm.SellerAccount);

            TempData["SuccessMessage"] = "驗證郵件已發送，請查收並完成驗證，5秒後導向登入頁面";
            return View(vm);

            //// 檢查是否有相同的帳號已經存在於資料庫中
            //var newAccount = db.Sellers.FirstOrDefault(x => x.SellerAccount == s.SellerAccount);
            //if (newAccount != null)
            //{
            //    TempData["Fail"] = "此帳號已經被註冊！";
            //    return View();
            //}
            //if (!IsStrongPassword(s.SellerPassword))
            //{
            //    TempData["Fail"] = "密碼需至少包含一個英文大寫、一個英文小寫、一個數字、一個符號，且至少8個字元！";
            //    return View();
            //}
            //if (photo != null && photo.ContentLength > 0)
            //{
            //    // 確認有上傳圖片
            //    // 獲取文件名和副檔名
            //    var fileName = Path.GetFileName(photo.FileName);
            //    var fileExtension = Path.GetExtension(fileName);

            //    // 生成唯一的文件名，以避免重複
            //    var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

            //    // 獲取伺服器上的路徑
            //    var serverPath = Server.MapPath("~/img/"); //儲存到 "img" 的資料夾中

            //    // 確保資料夾存在
            //    if (!Directory.Exists(serverPath))
            //    {
            //        Directory.CreateDirectory(serverPath);
            //    }

            //    // 保存圖片到伺服器
            //    var fullPath = Path.Combine(serverPath, uniqueFileName);
            //    photo.SaveAs(fullPath);

            //    // 將圖片路徑保存到資料庫中
            //    s.SellerImagePath = uniqueFileName;
            //}

            //if (String.IsNullOrEmpty(s.GUINumber))
            //{
            //    TempData["Fail"] = "統編為必填欄位！";
            //    return View();
            //}
            //if (String.IsNullOrEmpty(s.SellerName))
            //{
            //    TempData["Fail"] = "負責人姓名為必填欄位！";
            //    return View();
            //}
            //if (String.IsNullOrEmpty(s.StoreName))
            //{
            //    TempData["Fail"] = "商家姓名為必填欄位！";
            //    return View();
            //}
            //if (String.IsNullOrEmpty(s.SellerAccount))
            //{
            //    TempData["Fail"] = "電子郵件為必填欄位！";
            //    return View();
            //}
            //if (String.IsNullOrEmpty(s.SellerPassword))
            //{
            //    TempData["Fail"] = "使用者密碼為必填欄位！";
            //    return View();
            //}
            //if (String.IsNullOrEmpty(s.Address))
            //{
            //    TempData["Fail"] = "住址為必填欄位！";
            //    return View();
            //}
            //if (String.IsNullOrEmpty(s.Phone))
            //{
            //    TempData["Fail"] = "手機號碼為必填欄位！";
            //    return View();
            //}
            //if (String.IsNullOrEmpty(s.BankAccount))
            //{
            //    TempData["Fail"] = "銀行帳戶為必填欄位！";
            //    return View();
            //}
            //if (String.IsNullOrEmpty(s.StoreIntro))
            //{
            //    TempData["Fail"] = "商城介紹為必填欄位！";
            //    return View();
            //}
            //s.AccessRightId = 2;
            //db.Sellers.Add(s);
            //db.SaveChanges();
            //SendRegisterEmail(s.SellerAccount);
            ////TempData["Success"] = "您已註冊成功！";
            //// 假設註冊成功後，取得賣家的 ID 和商店名稱
            ////int sellerId = s.SellerId; // 這裡假設取得了賣家的 ID
            ////string storeName = s.StoreName; // 假設取得了商店名稱
            ////string sellerImagePath = s.SellerImagePath;
            //// 將賣家的 ID 和商店名稱存入 Session 中，方便在 SellerMain 頁面使用
            //// Session["SellerId"] = sellerId;
            ////Session["StoreName"] = storeName;
            ////Session["SellerImagePath"] = sellerImagePath;
            //// 註冊成功後，直接導向到 SellerMain 頁面，並將賣家的 ID 傳遞過去
            ////return RedirectToAction("Index", "SellerMain", new { id = sellerId });

            

            //TempData["SuccessMessage"] = "驗證郵件已發送，請查收並完成驗證，5秒後導向登入頁面";
            //return View(s);
        }

        private void SendRegisterEmail(string sellerAccount)
        {
            var db = new AppDbContext();
            var selleraccount = db.Sellers.FirstOrDefault(x => x.SellerAccount == sellerAccount);
            if (selleraccount != null)
            {
                string verificationToken = Guid.NewGuid().ToString();
                selleraccount.EmailCheck = verificationToken;
                db.SaveChanges();

                string relativeUrl = Url.Action("SellerRegisterEmail", "SellerRegister", new { token = verificationToken });
                string absoluteUrl = Request.Url.Scheme + "://" + Request.Url.Authority + relativeUrl;

                var fromAddress = new MailAddress("Eshopping17go@gmail.com", "E起購");
                var toAddress = new MailAddress(sellerAccount);
                string subject = "EShopping開通會員驗證信";
                string body = $"<html><body><h3>請點擊以下連結驗證您的電子郵件:<a href=\"{absoluteUrl}\">驗證</a></h3></body></html>";

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("Eshopping17go@gmail.com", "ayakelsjzapfbtil"),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true//可以吃到html標籤
                })
                {
                    smtpClient.Send(message);
                }
            }
        }
        public ActionResult SellerRegisterEmail(string token)
        {
            var db = new AppDbContext();
            var sellertoken = db.Sellers.FirstOrDefault(x => x.EmailCheck == token);
            if (sellertoken != null)
            {
                sellertoken.AccessRightId = 1;
                sellertoken.EmailCheck = null;
                db.SaveChanges();
                TempData["SuccessMessage"] = "您的郵箱已成功驗證";
            }
            else
            {
                TempData["ErrorMessage"] = "驗證已失敗";
            }
            return View();
        }

        private bool IsStrongPassword(string sellerPassword)
        {
            return Regex.IsMatch(sellerPassword, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&+=]).{8,}$");
        }
    }
}