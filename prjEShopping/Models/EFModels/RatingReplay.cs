namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RatingReplaies")]
    public partial class RatingReplay
    {
        [Key]
        public int RatingReplyId { get; set; }

        public int? RatingId { get; set; }

        [StringLength(4000)]
        public string ReplayText { get; set; }

        public DateTime? ReplayTime { get; set; }

        [StringLength(50)]
        public string ReplayStatus { get; set; }
    }
}
