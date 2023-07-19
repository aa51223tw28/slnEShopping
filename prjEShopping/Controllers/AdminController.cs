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

        [HttpPost]
        public ActionResult AdminRegister(AdminRegisterVM vm)
        {
            return View();
        }

        public ActionResult AdminEdit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminEdit(AdminVM vm)
        {
            return View();
        }

    }
}