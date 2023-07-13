namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FlashSaleAD
    {
        public int FlashSaleADId { get; set; }

        public int? ADProductId { get; set; }

        public int? SellerId { get; set; }

        public int? ProductId { get; set; }

        public DateTime? ADStartDate { get; set; }

        public DateTime? ADEndDate { get; set; }

        [MaxLength(50)]
        public byte[] ListingStatus { get; set; }
    }
}
