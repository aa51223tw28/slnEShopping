using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class SellerPasswordChangeVM
    {
            public int SellerId { get; set; }
            [Required]
        [Display(Name = "舊密碼")]
        public string OldPassword { get; set; }

            [Required]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,16}$", ErrorMessage = "密碼必須包括至少一個大寫字母、一個小寫字母和一個數字，且至少8個字符，最多16個字符。")]


        [Display(Name = "新密碼")]
        public string NewPassword { get; set; }
        [Display(Name = "確認密碼")]
        [Required]
        [Compare("NewPassword", ErrorMessage = "密碼不匹配。")]

            public string ConfirmPassword { get; set; }



    }
}