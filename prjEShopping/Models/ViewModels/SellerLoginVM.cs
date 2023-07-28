using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace prjEShopping.Models.ViewModels
{
    public class SellerLoginVM
    {
        public int SellerId { get; set; }   
        [Display(Name = "帳號")]
        [Required(ErrorMessage ="帳號為必填欄位")]
        public string SellerAccount { get; set; }

        [Display(Name = "密碼")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "密碼為必填欄位")]
        public string SellerPassword { get; set; }
        public string LoginErrorMessage { get; set; }
    }
}