using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class UserFollowProductVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }       
        public decimal Price { get; set; }
        public string ProductImagePathOne { get; set; }
        public int TrackProductId { get; set; }

    }
}