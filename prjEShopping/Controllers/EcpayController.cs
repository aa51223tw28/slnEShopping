using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class EcpayController : Controller
    {

        [Authorize]
        // GET: Ecpay
        public ActionResult Index(int orderId)
        {
            //var customerAccount = User.Identity.Name;
            //var db = new AppDbContext();
            //var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            //var orderdb = db.Orders.

            //需填入你的網址
            //var website = $"https://localhost:44325/";
            //var order = new Dictionary<string, string>
            //{
            //    { "MerchantTradeNo",  orderId},
            //    { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
            //    { "TotalAmount",  "100"},
            //    { "TradeDesc",  "無"},
            //    { "ItemName",  "測試商品"},
            //    { "ExpireDate",  "3"},
            //    { "CustomField1",  ""},
            //    { "CustomField2",  ""},
            //    { "CustomField3",  ""},
            //    { "CustomField4",  ""},
            //    { "ReturnURL",  $"{website}/api/Ecpay/AddPayInfo"},
            //    { "OrderResultURL", $"{website}/Home/PayInfo/{orderId}"},
            //    { "PaymentInfoURL",  $"{website}/api/Ecpay/AddAccountInfo"},
            //    { "ClientRedirectURL",  $"{website}/Home/AccountInfo/{orderId}"},
            //    { "MerchantID",  "2000132"},
            //    { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},
            //    { "PaymentType",  "aio"},
            //    { "ChoosePayment",  "ALL"},
            //    { "EncryptType",  "1"},
            //};
            //檢查碼
            //order["CheckMacValue"] = GetCheckMacValue(order);
            //return View(order);
            return View();
        }
    }
}