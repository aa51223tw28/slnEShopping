using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class RatingVM
    {
        public int RatingId { get; set; }
        public int StarRating { get; set; }
        public string RatingText { get; set; }
        public DateTime PostTime { get; set; }
        public string SellerReply { get; set; } // 新增用來顯示賣家回覆的屬性
    }
}