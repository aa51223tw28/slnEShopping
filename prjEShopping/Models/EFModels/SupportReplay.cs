namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SupportReplaies")]
    public partial class SupportReplay
    {
        public int SupportReplayId { get; set; }

        public int? SupportId { get; set; }

        public int? AdminId { get; set; }

        public int? SellerId { get; set; }

        public int? UserId { get; set; }

        [StringLength(50)]
        public string ReplayTitle { get; set; }

        [StringLength(4000)]
        public string ReplayText { get; set; }

        public DateTime? ReplayTime { get; set; }

        [StringLength(50)]
        public string SupportReplayStatus { get; set; }

        [StringLength(100)]
        public string ImageLink { get; set; }
    }
}
