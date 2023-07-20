using Microsoft.Win32;
using prjEShopping.Models;
using prjEShopping.Models.DTOs;
using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.Json;

namespace prjEShopping.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult List()
        {
            var db = new AppDbContext();
            List<Admin> admins = db.Admins.ToList();
            var adminDto = admins.Select(a => new AdminDto
            {
                AdminId = a.AdminId,
                AdminNumber = a.AdminNumber,
                PermissionsId = a.PermissionsId,
                AdminAccount = a.AdminAccount,
                AdminPassword = a.AdminPassword,
                Title = a.Title,
                AdminName = a.AdminName,
                Phone = a.Phone,
                DateOfHire = a.DateOfHire,
                JobStatus = a.JobStatus
            }).ToList();

            var adminViewModels = adminDto.Select(a => new AdminVM
            {
                AdminNumber = a.AdminNumber,
                PermissionsId = a.PermissionsId,
                AdminAccount = a.AdminAccount,
                AdminPassword = a.AdminPassword,
                Title = a.Title,
                AdminName = a.AdminName,
                Phone = a.Phone,
                DateOfHire = a.DateOfHire,
                JobStatus = a.JobStatus
            }).ToList();

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

            return View(adminViewModels);


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
        public ActionResult AdminEdit(AdminVM vm)
        {
            return View();
        }

        public ActionResult AdminEdit2(AdminVM am)
        {
            return View(am);
        }

    }
}