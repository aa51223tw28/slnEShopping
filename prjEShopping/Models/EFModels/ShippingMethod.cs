namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ShippingMethod
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ShippingMethodId { get; set; }

        [StringLength(50)]
        public string ShippingMethodName { get; set; }

        [Column(TypeName = "money")]
        public decimal? Freight { get; set; }
    }
}
