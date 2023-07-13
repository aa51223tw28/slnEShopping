namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ADProduct
    {
        public int ADProductId { get; set; }

        [StringLength(50)]
        public string ADName { get; set; }

        public int? ADPoint { get; set; }

        public DateTime? ADStartDate { get; set; }

        public DateTime? ADEndDate { get; set; }

        public int? ADQuantity { get; set; }

        [StringLength(50)]
        public string ADImagePath { get; set; }

        public int? ProductId { get; set; }
    }
}
