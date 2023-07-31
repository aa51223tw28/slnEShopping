using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class UserFeedbackVM
    {
        // 賣家資訊
        public int SellerId { get; set; }
        public string SellerName { get; set; }

        // 評分清單
        public List<Rating> Ratings { get; set; }

        // 留言清單
        public List<Rating> Comments { get; set; }
    }
}