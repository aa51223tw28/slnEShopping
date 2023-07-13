namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Rating
    {
        public int RatingId { get; set; }

        public int? ProductId { get; set; }

        public int? UserId { get; set; }

        [StringLength(50)]
        public string StarRating { get; set; }

        [StringLength(4000)]
        public string RatingText { get; set; }

        public DateTime? PostTime { get; set; }

        [StringLength(50)]
        public string RatingStatus { get; set; }
    }
}
