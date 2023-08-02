namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SellersAD
    {
        public int SellersADId { get; set; }

        public int? SellerId { get; set; }

        public int? ADProductId { get; set; }
    }
}
