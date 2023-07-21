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
        [Display(Name = "帳號")]
        [Required]
        public string SellerAccount { get; set; }

        [Display(Name = "密碼")]
        [Required]
        public string SellerPassword { get; set; }
    }
}