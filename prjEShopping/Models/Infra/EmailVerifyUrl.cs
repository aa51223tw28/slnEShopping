using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Security.Policy;
using System.Web.Mvc;

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
            string body = CreateUrl(urlHelper, action, controllers);
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

        public static string CreateUrl(UrlHelper urlHelper,string action,string controllers)
        {
            string token = Guid.NewGuid().ToString();
            string verificationUrl = urlHelper.Action(action, controllers, new { token }, urlHelper.RequestContext.HttpContext.Request.Url.Scheme);
            return verificationUrl;
        }
    }
}