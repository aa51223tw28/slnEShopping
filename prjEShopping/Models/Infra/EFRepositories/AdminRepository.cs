using prjEShopping.Models.DTOs;
using prjEShopping.Models.EFModels;
using prjEShopping.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.Infra.EFRepositories
{
    public class AdminRepository : IAdminRepository

    {
        private AppDbContext _db;
        public AdminRepository()
        {
            _db = new AppDbContext();
        }
        public IEnumerable<AdminDto> Search()
        {
            return _db.Admins.Select(a => new AdminDto
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
            });
 
        }
    }
}