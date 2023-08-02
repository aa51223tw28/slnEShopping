﻿using Microsoft.Win32;
using prjEShopping.Models;
using prjEShopping.Models.DTOs;
using prjEShopping.Models.EFModels;
using prjEShopping.Models.Infra.EFRepositories;
using prjEShopping.Models.Interface;
using prjEShopping.Models.Services;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace prjEShopping.Controllers
{
    public class AdmintestController : Controller
    {
        [HttpGet]
        public ActionResult SendEmail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendEmail(string email)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("Eshopping17go@gmail.com", "ayakelsjzapfbtil"),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            //圖片前置
            string htmlBody = "<html><body><h1>測試郵件</h1><img src=\"cid:MyImage\" /></body></html>";
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");
            // 嵌入圖片
            LinkedResource linkedImage = new LinkedResource("~/img/Etest.jpg");
            linkedImage.ContentId = "MyImage";
            htmlView.LinkedResources.Add(linkedImage);
         
            string body =CreateUrl();

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("Eshopping17go@gmail.com"),
                Subject = "EShopping帳號驗證郵件",
                Body ="EShopping 驗證連結，請點選驗證"+body,
            };

            mailMessage.AlternateViews.Add(htmlView);
            mailMessage.To.Add(email);
            smtpClient.Send(mailMessage);
            mailMessage.IsBodyHtml = true;

            ViewBag.Message = "郵件已發送!";
            return View();
        }

        public ActionResult VEmail(string token)
        {
                return View();
        }

            public string CreateUrl()
            {
                string token = Guid.NewGuid().ToString();
                string verificationUrl = Url.Action("VEmail", "Admintest", new { token }, Request.Url.Scheme);
                return verificationUrl;
            }

        // GET: Admin
        public ActionResult List()
        {
            IEnumerable<AdminVM> admin = GetAdmins();

            var db = new AppDbContext();
            List<string> adminNumbers = db.Admins.Select(a => a.AdminNumber).ToList();

            var date = DateTime.Now;
            string year = date.Year.ToString().Substring(2, 2);
            string month = date.Month.ToString().PadLeft(2, '0');
            string day = date.Day.ToString().PadLeft(2, '0');

            int count = 1;
            string adminNumber = GenerateAdminNumber(year, month, day, count);

            while (adminNumbers.Contains(adminNumber))
            {
                count++;
                adminNumber = GenerateAdminNumber(year, month, day, count);
            }

            ViewBag.AdminNumber = adminNumber;

            return View(admin);


        }

        [HttpPost]
        public ActionResult List(AdminVM vm)
        {
            AppDbContext db = new AppDbContext();
            var data = db.Admins.FirstOrDefault(a => a.AdminAccount == vm.AdminAccount);
            if(data!=null)
            {
                ModelState.AddModelError(string.Empty, "這個信箱已註冊，請用其他信箱進行申請帳號!");
            }
            if (ModelState.IsValid)
            {
                var datas = new Admin
                {
                    AdminId = vm.AdminId,
                    AdminNumber = vm.AdminNumber,
                    PermissionsId = vm.PermissionsId,
                    AdminAccount = vm.AdminAccount,
                    AdminPassword = vm.AdminPassword,
                    Title = vm.Title,
                    AdminName = vm.AdminName,
                    Phone = vm.Phone,
                    DateOfHire = vm.DateOfHire,
                    JobStatus = vm.JobStatus
                };
                db.Admins.Add(datas);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            return View();
        }

        private IEnumerable<AdminVM> GetAdmins()
        {
            IAdminRepository repo = new AdminRepository();

            AdminService service = new AdminService(repo);
            return service.Search()
                .Select(dto => new AdminVM
                {
                    AdminId = dto.AdminId,
                    AdminNumber = dto.AdminNumber,
                    PermissionsId = dto.PermissionsId,
                    AdminAccount = dto.AdminAccount,
                    AdminPassword = dto.AdminPassword,
                    Title = dto.Title,
                    AdminName = dto.AdminName,
                    Phone = dto.Phone,
                    DateOfHire = dto.DateOfHire,
                    JobStatus = dto.JobStatus
                });
        }

        public ActionResult AdminLogin()
        {
            return View();
        }


        public ActionResult AdminRegister()
        {
          
            return View();
        }

        public string GenerateAdminNumber(string year, string month, string day, int count)
        {
            return $"A{year}{month}{day}{count.ToString().PadLeft(2, '0')}";
        }

        [HttpPost]
        public ActionResult AdminRegister(Admin admin)
        {
            AppDbContext db = new AppDbContext();
            db.Admins.Add(admin);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        public ActionResult AdminEdit(int? id)
        {
            //if (id == null)
            //    return RedirectToAction("List");
            var db=new AppDbContext();
            var _adminId = db.Admins.FirstOrDefault(p => p.AdminId == 1);
            return View(_adminId);
        }

        [HttpPost]
        public ActionResult AdminEdit(Admin admin)
        {
            var db = new AppDbContext();
            var dbAdmin = db.Admins.FirstOrDefault(a => a.AdminId == admin.AdminId);
            if (dbAdmin == null)
            {
                // Handle the case where the admin was not found in the database
                return HttpNotFound();
            }
            if (TryUpdateModel(dbAdmin))
            {
                db.SaveChanges();
                return RedirectToAction("List");
            }
            else
            {
                // Handle the case where the model was invalid
                return View(admin);
            }
        }

        public ActionResult AdminEdit2(AdminVM am)
        {
            return View(am);
        }

    }
}