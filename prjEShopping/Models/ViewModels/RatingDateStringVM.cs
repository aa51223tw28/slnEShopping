using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class RatingDateStringVM
    {
        public int? RatingId { get; set; }
        public int StarRating { get; set; }
        public string RatingText { get; set; }
        public string PostTime { get; set; }

        public string ReplayText { get; set; }
        public string ReplayTime { get; set;}
        [StringLength(50)]
        public int UserId { get; set; }
        [StringLength(50)]
        public string UserImagePath { get; set; }
        //   public RatingReplay SellerReply { get; set; }
    }
}