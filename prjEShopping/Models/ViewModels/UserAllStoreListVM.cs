using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class UserAllStoreListVM
    {
        public int SellerId { get; set; }

        public string StoreName { get; set; }

        public string SellerImagePath { get; set; }

        public int TrackCount { get; set; }
    }
}