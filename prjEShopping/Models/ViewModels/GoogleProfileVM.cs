﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class GoogleProfileVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Email { get; set; }
        public string Verified_Email { get; set; }
        public string MobilePhone { get; set; }
        public string Locale { get; set; }
    }
}