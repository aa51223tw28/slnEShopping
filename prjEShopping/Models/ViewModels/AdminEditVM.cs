﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class AdminEditVM
    {
        public int AdminId { get; set; }

        [DisplayName("職員編號")]
        [StringLength(50)]
        public string AdminNumber { get; set; }

        [DisplayName("權限等級")]
        public int? PermissionsId { get; set; }

        [DisplayName("帳號")]
        [StringLength(50)]
        public string AdminAccount { get; set; }

        [DisplayName("密碼")]
        [StringLength(50)]
        public string AdminPassword { get; set; }

        [DisplayName("職稱")]
        [StringLength(50)]
        public string Title { get; set; }

        [DisplayName("姓名")]
        [StringLength(50)]
        public string AdminName { get; set; }

        [DisplayName("電話")]
        [StringLength(50)]
        public string Phone { get; set; }

        [DisplayName("入職時間")]
        public DateTime? DateOfHire { get; set; }

        [DisplayName("入職狀態")]
        [StringLength(50)]
        public string JobStatus { get; set; }
    }
}