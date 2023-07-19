using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
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
}