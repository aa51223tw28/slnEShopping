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
        public string UserName { get; set; }
        public string UserAccount { get; set; }       
        public string UserPassword { get; set; }
        public string UserImagePath { get; set; }
    }
}