﻿using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Imaging;
using System.Linq;
using System.Security;
using System.Web;
using static System.Web.Razor.Parser.SyntaxConstants;

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

        //如沒有要做資料庫外部關聯 可以這樣簡單做
        public string PermissionsName
        {
            get
            {
                switch (PermissionsId)
                {
                    case 1:
                        return "總管理員";
                    case 2:
                        return "管理員";
                    case 3:
                        return "平台管理";
                    case 4:
                        return "會員管理";
                    case 5:
                        return "行銷管理";
                    case 6:
                        return "客服管理";
                    default:
                        return "未知";
                }
            }
        }

    [DisplayName("帳號")]
        [StringLength(50)]
        [Required]
        public string AdminAccount { get; set; }

        [DisplayName("密碼")]
        [StringLength(100)]
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

        public int? AccessRightId { get; set; }

    }

    public class AdminLoginVM
    {

        [DisplayName("帳號")]
        [StringLength(50)]
        [Required]
        public string AdminAccount { get; set; }

        [DisplayName("密碼")]
        [StringLength(100)]
        [Required]
        public string AdminPassword { get; set; }
    }

    public class PasswordChangeVM
    {
        public int UserId { get; set; }
        [Required]
        public string OldPassword { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,16}$", ErrorMessage = "密碼必須包括至少一個大寫字母、一個小寫字母和一個數字，且至少8個字符，最多16個字符。")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "密碼不匹配。")]
        public string ConfirmNewPassword { get; set; }
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
                AccessRightId = vm.AccessRightId,
            };
        }

            public static Admin VM2AdminEdit (AdminVM vm)
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
                AccessRightId = vm.AccessRightId,
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
                AccessRightId = admin.AccessRightId,
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