using Microsoft.Win32;
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
using System.Web;
using System.Web.Mvc;


namespace prjEShopping.Controllers
{
    public class AdminController : Controller
    {
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