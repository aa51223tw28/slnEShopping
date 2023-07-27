using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Imaging;
using System.Linq;
using System.Security;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class AdminVM
    {
        public int AdminId { get; set; }
        [DisplayName("職員編號")]
        [StringLength(50)]
        public string AdminNumber { get; set; }

        [DisplayName("權限等級")]
        [Required]
        public int? PermissionsId { get; set; }

        [DisplayName("帳號")]
        [StringLength(50)]
        [Required]
        public string AdminAccount { get; set; }

        [DisplayName("密碼")]
        [StringLength(50)]
        [Required]
        public string AdminPassword { get; set; }

        [StringLength(100)]
        public string AdminPasswordSalt { get; set; }

        [DisplayName("職稱")]
        [StringLength(50)]
        [Required]
        public string Title { get; set; }

        [DisplayName("姓名")]
        [StringLength(50)]
        [Required]
        public string AdminName { get; set; }

        [DisplayName("電話")]
        [StringLength(10)]
        [Required]
        public string Phone { get; set; }

        [DisplayName("入職時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime? DateOfHire { get; set; }

        [DisplayName("在職狀態")]
        [StringLength(50)]
        [Required]
        public string JobStatus { get; set; }

        [StringLength(50)]
        public string Role { get; set; }

    }

    public class AdminLoginVM
    {

        [DisplayName("帳號")]
        [StringLength(50)]
        [Required]
        public string AdminAccount { get; set; }

        [DisplayName("密碼")]
        [StringLength(50)]
        [Required]
        public string AdminPassword { get; set; }
    }

    public static class AdminChange
    {
        public static Admin VM2Admin(AdminVM vm)
        {
            return new Admin
            {
                AdminId = vm.AdminId,
                AdminNumber = vm.AdminNumber,
                PermissionsId = vm.PermissionsId,
                AdminAccount = vm.AdminAccount,
                AdminPassword = vm.AdminPassword,
                AdminPasswordSalt = vm.AdminPasswordSalt,
                Title = vm.Title,
                AdminName = vm.AdminName,
                Phone = vm.Phone,
                DateOfHire = vm.DateOfHire,
                JobStatus = vm.JobStatus,
                Role = vm.Role,
            };
        }

        public static AdminVM Admin2VM(Admin admin)
        {
            return new AdminVM
            {
                AdminId = admin.AdminId,
                AdminNumber = admin.AdminNumber,
                PermissionsId = admin.PermissionsId,
                AdminAccount = admin.AdminAccount,
                AdminPassword = admin.AdminPassword,
                AdminPasswordSalt = admin.AdminPasswordSalt,
                Title = admin.Title,
                AdminName = admin.AdminName,
                Phone = admin.Phone,
                DateOfHire = admin.DateOfHire,
                JobStatus = admin.JobStatus,
                Role = admin.Role,
            };
        }

        public static List<AdminVM> Admin2VM(this IEnumerable<Admin> source)
        {
            if (source == null || source.Count() == 0)
            {
                return Enumerable.Empty<AdminVM>().ToList();
            }

            return source.Select(s => Admin2VM(s)).ToList();
        }
    }
}