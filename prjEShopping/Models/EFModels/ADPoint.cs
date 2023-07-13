namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ADPoint
    {
        public int ADPointId { get; set; }

        public int? SellerId { get; set; }

        [StringLength(50)]
        public string GUINumber { get; set; }

        public DateTime? PurchaseTime { get; set; }

        public int? ADPoints { get; set; }

        [StringLength(50)]
        public string PaymentStatus { get; set; }
    }
}
