using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class SellerReplyVM
    {
        public int RatingReplyId { get; set; }
        public int RatingId { get; set; }
        public string ReplayText { get; set; }
        public DateTime ReplayTime { get; set; }
        public string ReplayStatus { get; set; }
    }
}