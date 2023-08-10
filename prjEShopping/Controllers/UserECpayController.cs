using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;
using System.Web.Mvc;
using System.Windows;

namespace prjEShopping.Controllers
{
    public class UserECpayController : Controller
    {
        [Authorize]
        // GET: UserECpay
        public ActionResult ToECpay()
        {
            //要從資料庫撈資料傳去綠界            
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var orderdbNum = db.Orders.Where(x=>x.UserId== userid).OrderByDescending(x => x.OrderId).Select(x => x.OrderNumber).FirstOrDefault();
            var orderguid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);

            var orderNum = orderdbNum + orderguid;//總共20個字元為何給綠界用 需要不重複 以免付款失敗

            //總價
            var orderid = db.Orders.Where(x => x.OrderNumber == orderdbNum).Select(x => x.OrderId).FirstOrDefault();
            var totalprice = db.OrderDetails.Where(x => x.OrderId == orderid).Sum(x => x.CurrentPrice * x.Quantity);
            int total = (int)Math.Round((decimal)totalprice);

            //買了什麼商品
            var items = db.OrderDetails.Where(x => x.OrderId == orderid)
                                        .Join(db.Products, x => x.ProductId, y => y.ProductId, (x,y) => new
                                        {
                                            ProductId=x.ProductId,
                                            Quantity=x.Quantity,
                                            ProductName =y.ProductName
                                        })
                                        .Select(x => x.ProductName + "X" + x.Quantity);
            string itemName = string.Join("#", items);

            //需填入你的網址
            var website = $"https://localhost:44388/";
            var order = new Dictionary<string, string>
            {
                //特店交易編號
                { "MerchantTradeNo",  orderNum},
                
                //特店交易時間
                { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
                
                //交易金額
                { "TotalAmount",  total.ToString()},
                
                //交易描述
                { "TradeDesc",  "無"},
                
                //商品名稱
                { "ItemName",  itemName},
                
                //允許繳費有效天數
                { "ExpireDate",  "3"},
                
                { "CustomField1",  ""},
                { "CustomField2",  ""},
                { "CustomField3",  ""},
                { "CustomField4",  ""},
               
                //綠界回傳付款資訊的至 此URL
                { "ReturnURL",  $"{website}api/Ecpay/AddPayInfo"},//目前沒用到
                
                //使用者於綠界 付款完成後，綠界將會轉址至 此URL
                { "OrderResultURL", $"{website}UserECpay/PayInfo"},
                
                //付款方式為 ATM 時，當使用者於綠界操作結束時，綠界回傳 虛擬帳號資訊至 此URL
                { "PaymentInfoURL",  $"{website}/api/Ecpay/AddAccountInfo"},//目前沒用到
                
                //付款方式為 ATM 時，當使用者於綠界操作結束時，綠界會轉址至 此URL。
                { "ClientRedirectURL",  $"{website}/Home/AccountInfo/{orderNum}"},//目前沒用到
                
                //特店編號， 2000132 測試綠界編號
                { "MerchantID",  "2000132"},
               
                //忽略付款方式
                { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},
                
                //交易類型 固定填入 aio
                { "PaymentType",  "aio"},
                
                //選擇預設付款方式 固定填入 ALL
                { "ChoosePayment",  "ALL"},
                
                //CheckMacValue 加密類型 固定填入 1 (SHA256)
                { "EncryptType",  "1"},
            };
            //檢查碼
            order["CheckMacValue"] = GetCheckMacValue(order);

            return View(order);
        }

        private string GetCheckMacValue(Dictionary<string, string> order)
        {
            var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();

            var checkValue = string.Join("&", param);

            //測試用的 HashKey
            var hashKey = "5294y06JbISpM5x9";

            //測試用的 HashIV
            var HashIV = "v77hoKGq4kWxNNIS";

            checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={HashIV}";

            checkValue = HttpUtility.UrlEncode(checkValue).ToLower();

            checkValue = GetSHA256(checkValue);

            return checkValue.ToUpper();
        }
        private string GetSHA256(string value)
        {
            var result = new StringBuilder();
            var sha256 = SHA256Managed.Create();
            var bts = Encoding.UTF8.GetBytes(value);
            var hash = sha256.ComputeHash(bts);

            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }

            return result.ToString();
        }

        [Authorize]
        public ActionResult PayInfo()
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var orderdbNum = db.Orders.Where(x => x.UserId == userid).OrderByDescending(x => x.OrderId).Select(x => x.OrderNumber).FirstOrDefault();
            ViewBag.OrderNumber = orderdbNum;
            return View();
        }
    }
}