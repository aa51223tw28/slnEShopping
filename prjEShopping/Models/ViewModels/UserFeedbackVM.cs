using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class UserFeedbackVM
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

    }
}