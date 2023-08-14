using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Windows.Media;
using System.ComponentModel;
using prjEShopping.Models.EFModels;
using System.Net;

namespace prjEShopping.Models.ViewModels
{
    public class AdminUserVM
    {

        [DisplayName("用戶ID")]
        public int UserId { get; set; }

        [DisplayName("用戶名")]
        [StringLength(50)]
        public string UserName { get; set; }

        [DisplayName("用戶帳號")]
        [StringLength(50)]
        public string UserAccount { get; set; }

        [DisplayName("用戶密碼")]
        [StringLength(100)]
        public string UserPassword { get; set; }

        [StringLength(100)]
        public string UserPasswordSalt { get; set; }

        [DisplayName("性別")]
        [StringLength(50)]
        public string Gender { get; set; }

        [DisplayName("電話")]
        [StringLength(50)]
        public string Phone { get; set; }

        [DisplayName("手機")]
        [StringLength(50)]
        public string CellPhone { get; set; }

        [DisplayName("城市")]
        [StringLength(50)]
        public string City { get; set; }

        [DisplayName("地址")]
        [StringLength(50)]
        public string Address { get; set; }

        [DisplayName("貨運方式")]
        [StringLength(50)]
        public int? ShippingMethodId { get; set; }

        [DisplayName("付款方式")]
        [StringLength(50)]
        public int? PaymenyMethodId { get; set; }

        [DisplayName("生日")]
        [Column(TypeName = "date")]
        public DateTime? Birthday { get; set; }

        [DisplayName("權限")]
        [StringLength(50)]
        public int? AccessRightId { get; set; }

        [StringLength(50)]
        public string UserImagePath { get; set; }

        [StringLength(50)]
        public string Role { get; set; }

        [StringLength(100)]
        public string EmailCheck { get; set; }
    }

    public static class AdminUserChange
    {
        public static AdminUserVM AdminUser2VM(User u)
        {
            return new AdminUserVM
            {
                UserId = u.UserId,
                UserName = u.UserName,
                UserAccount = u.UserAccount,
                UserPassword = u.UserPassword,
                UserPasswordSalt = u.UserPasswordSalt,
                Gender = u.Gender,
                Phone = u.Phone,
                CellPhone = u.CellPhone,
                City = u.City,
                Address = u.Address,
                //ShippingMethodId = u.ShippingMethodId,
                //PaymenyMethodId = u.PaymenyMethodId,
                Birthday = u.Birthday,
                //AccessRightId = u.AccessRightId,
                UserImagePath = u.UserImagePath,
                Role = u.Role,
                EmailCheck = u.EmailCheck,
            };
        }

        public static List<AdminUserVM> AdminUser2VM(this IEnumerable<User> source)
        {
            if (source == null || source.Count() == 0)
            {
                return Enumerable.Empty<AdminUserVM>().ToList();
            }

            return source.Select(p => AdminUser2VM(p)).ToList();
        }

        public static User VM2AdminUser(AdminUserVM vm)
        {
            return new User
            {
                UserId = vm.UserId,
                UserName = vm.UserName,
                UserAccount = vm.UserAccount,
                UserPassword = vm.UserPassword,
                UserPasswordSalt = vm.UserPasswordSalt,
                Gender = vm.Gender,
                Phone = vm.Phone,
                CellPhone = vm.CellPhone,
                City = vm.City,
                Address = vm.Address,
                //ShippingMethodId = vm.ShippingMethodId,
                //PaymenyMethodId = vm.PaymenyMethodId,
                Birthday = vm.Birthday,
                //AccessRightId = vm.AccessRightId,
                UserImagePath = vm.UserImagePath,
                Role = vm.Role,
                EmailCheck = vm.EmailCheck,
            };
        }
    }
}