using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Security.Policy;
using System.Web.Mvc;
using prjEShopping.Models.EFModels;

namespace prjEShopping.Models.Infra
{
    public static class EmailVerifyUrl
    {
        public static void SendEmailUrl(string email, UrlHelper urlHelper , string action, string controllers)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("Eshopping17go@gmail.com", "ayakelsjzapfbtil"),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            //html前置
            string body = CreateUrl(email,urlHelper, action, controllers);
            string htmlBody = $"<html><body><h1>驗證確認</h1><h3>EShopping 驗證連結，<a href=\"{body}\">請點選驗證</a></h3></body></html>";
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("Eshopping17go@gmail.com"),
                Subject = "EShopping帳號驗證郵件",
            };
            mailMessage.IsBodyHtml = true;
            mailMessage.AlternateViews.Add(htmlView);
            mailMessage.To.Add(email);
            smtpClient.Send(mailMessage);
        }

        public static string CreateUrl(string Account,UrlHelper urlHelper,string action,string controllers)
        {
            //產生確認碼
            string token = Guid.NewGuid().ToString();

            //變更權限
            var db = new AppDbContext();
            var account = db.Admins.FirstOrDefault(a => a.AdminAccount == Account);
            if (account != null)
            {
                account.AccessRightId = 3;
                account.EmailCheck = token;
                db.SaveChanges();
            }
            string verificationUrl = urlHelper.Action(action, controllers, new { token }, urlHelper.RequestContext.HttpContext.Request.Url.Scheme);
            return verificationUrl;
        }
    }
}