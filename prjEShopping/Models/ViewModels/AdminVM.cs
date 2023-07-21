using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        [Required]
        public DateTime? DateOfHire { get; set; }

        [DisplayName("在職狀態")]
        [StringLength(50)]
        [Required]
        public string JobStatus { get; set; }
    }
}