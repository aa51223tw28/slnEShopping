using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace prjEShopping.Models.ViewModels
{
    public class UserProfileVM
    {
        [Display(Name = "姓名")]
        [Required]
        public string UserName { get; set; }
        
        [Display(Name = "使用者帳號")]
        [Required]
        public string UserAccount { get; set; }
       
        [Display(Name = "密碼")]
        [Required]
        public string UserPassword { get; set; }
        
        [Display(Name = "確認密碼")]
        [Required]
        [Compare("UserPassword")]
        public string ConfirmUserPassword { get; set; }
        

        public string UserImagePath { get; set; }
        public HttpPostedFileBase UserImageFile { get; set; }

        [Required]
        [Display(Name = "手機號碼")]
        public string CellPhone { get; set; }

        [Required]
        [Display(Name = "電話號碼")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "姓別")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "通訊地(區域)")]
        public string City { get; set; }

        [Required]
        [Display(Name = "通訊地(街道)")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "通訊地(生日)")]
        public DateTime Birthday { get; set; }
    }
}