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
        public HttpPostedFileBase UserImageFile { get; set; }
        public string CellPhone { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public DateTime Birthday { get; set; }
    }
}