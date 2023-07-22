namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        public int OrderId { get; set; }

        public int? UserId { get; set; }

        public int? CouponId { get; set; }

        public DateTime? OrderDate { get; set; }

        [StringLength(50)]
        public string OrderNumber { get; set; }
    }
}
