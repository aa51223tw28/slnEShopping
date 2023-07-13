namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SliderAD
    {
        public int SliderADId { get; set; }

        public int? ADProductId { get; set; }

        public int? SellerId { get; set; }

        [StringLength(50)]
        public string ADImagePath { get; set; }

        public DateTime? ADStartDate { get; set; }

        public DateTime? ADEndDate { get; set; }

        [StringLength(50)]
        public string ListingStatus { get; set; }
    }
}
