using prjEShopping.Models;
using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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
        public ActionResult Create(Seller s, HttpPostedFileBase photo)
        {
            var db = new AppDbContext();
            
            // 檢查是否有相同的帳號已經存在於資料庫中
            var newAccount = db.Sellers.FirstOrDefault(x => x.SellerAccount == s.SellerAccount);
            if (newAccount != null)
            {
                TempData["Fail"] = "此帳號已經被註冊！";
                return View();
            }
            if (photo != null && photo.ContentLength > 0)
            {
                // 確認有上傳圖片
                // 獲取文件名和副檔名
                var fileName = Path.GetFileName(photo.FileName);
                var fileExtension = Path.GetExtension(fileName);

                // 生成唯一的文件名，以避免重複
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

                // 獲取伺服器上的路徑
                var serverPath = Server.MapPath("~/img/"); //儲存到 "img" 的資料夾中

                // 確保資料夾存在
                if (!Directory.Exists(serverPath))
                {
                    Directory.CreateDirectory(serverPath);
                }

                // 保存圖片到伺服器
                var fullPath = Path.Combine(serverPath, uniqueFileName);
                photo.SaveAs(fullPath);

                // 將圖片路徑保存到資料庫中
                s.SellerImagePath = uniqueFileName;
            }

            if (String.IsNullOrEmpty(s.GUINumber))
            {
                TempData["Fail"] = "統編為必填欄位！";
                return View();
            }
            if (String.IsNullOrEmpty(s.SellerName))
            {
                TempData["Fail"] = "負責人姓名為必填欄位！";
                return View();
            }
            if (String.IsNullOrEmpty(s.StoreName))
            {
                TempData["Fail"] = "商家姓名為必填欄位！";
                return View();
            }
            if (String.IsNullOrEmpty(s.SellerAccount))
            {
                TempData["Fail"] = "電子郵件為必填欄位！";
                return View();
            }
            if (String.IsNullOrEmpty(s.SellerPassword))
            {
                TempData["Fail"] = "使用者密碼為必填欄位！";
                return View();
            }
            if (String.IsNullOrEmpty(s.Address))
            {
                TempData["Fail"] = "住址為必填欄位！";
                return View();
            }
            if (String.IsNullOrEmpty(s.Phone))
            {
                TempData["Fail"] = "手機號碼為必填欄位！";
                return View();
            }
            if (String.IsNullOrEmpty(s.BankAccount))
            {
                TempData["Fail"] = "銀行帳戶為必填欄位！";
                return View();
            }
            if (String.IsNullOrEmpty(s.StoreIntro))
            {
                TempData["Fail"] = "商城介紹為必填欄位！";
                return View();
            }
            db.Sellers.Add(s);
            db.SaveChanges();
            TempData["Success"] = "您已註冊成功！";
            // 假設註冊成功後，取得賣家的 ID 和商店名稱
            int sellerId = s.SellerId; // 這裡假設取得了賣家的 ID
            string storeName = s.StoreName; // 假設取得了商店名稱
            string sellerImagePath = s.SellerImagePath;
            // 將賣家的 ID 和商店名稱存入 Session 中，方便在 SellerMain 頁面使用
            Session["SellerId"] = sellerId;
            Session["StoreName"] = storeName;
            Session["SellerImagePath"] = sellerImagePath;
            // 註冊成功後，直接導向到 SellerMain 頁面，並將賣家的 ID 傳遞過去
            return RedirectToAction("Index", "SellerMain", new { id = sellerId });
        }
    }
}