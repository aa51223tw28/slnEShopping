using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class Comment
    {
        public int RatingId { get; set; }

        public int? ProductId { get; set; }

        public int? UserId { get; set; }

        public int StarRating { get; set; }

        [StringLength(4000)]
        public string RatingText { get; set; }

        public DateTime? PostTime { get; set; }

        [StringLength(50)]
        public string RatingStatus { get; set; }
        public int RatingReplyId { get; set; }
        public string ReplayText { get; set; }
        public DateTime ReplayTime { get; set; }
        public string ReplayStatus { get; set; }
    }
}