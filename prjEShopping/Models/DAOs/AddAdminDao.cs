using prjEShopping.Models.DTOs;
using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;

namespace prjEShopping.Models.DAOs
{
    public class AddAdminDao
    {
        public void AddAdmin(AdminDto dto)
        {
            AppDbContext db = new AppDbContext();
            var admin = new Admin()
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
            };

            db.Admins.Add(admin);
            db.SaveChanges();
        }
    }
}