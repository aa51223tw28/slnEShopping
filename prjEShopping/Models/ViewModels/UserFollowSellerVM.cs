using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class UserFollowSellerVM
    {
        public int SellerId { get; set; }
        public string SellerName { get; set; }        
        public string SellerImagePath { get; set; }
        public int TrackSellerId { get; set; }
    }
}