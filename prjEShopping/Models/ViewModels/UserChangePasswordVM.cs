using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class UserChangePasswordVM
    {
        [Display(Name = "舊密碼")]
        [Required]
        public string OldPassword { get; set; }

        [Display(Name = "新密碼")]
        [Required]
        public string NewPassword { get; set; }

        [Display(Name = "確認密碼")]
        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}