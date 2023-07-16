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
            string t01 = "分支01";
            string t02 = "分支02修改";
            string t03 = "03修改"; //"03我也修改!";
            var db= new AppDbContext();
            List<Admin> admins = db.Admins.ToList();
            var adminDTOs = admins.Select(a => new AdminDto
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

            var adminViewModels = adminDTOs.Select(a => new AdminVM
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

    }
}