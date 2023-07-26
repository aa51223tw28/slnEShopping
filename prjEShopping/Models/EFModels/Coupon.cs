namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Coupon
    {
        public int CouponId { get; set; }

        public int? SellerId { get; set; }

        [StringLength(50)]
        public string CouponNumber { get; set; }

        [StringLength(50)]
        public string CouponName { get; set; }

        [StringLength(50)]
        public string CouponDetails { get; set; }

        public int? Quantity { get; set; }

        public int? ReceivedQuantity { get; set; }

        [StringLength(50)]
        public string CouponTerms { get; set; }

        [StringLength(50)]
        public string CouponType { get; set; }

        [StringLength(50)]
        public string Discount { get; set; }

        [StringLength(50)]
        public string Storewide { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? ClaimDeadline { get; set; }

        public DateTime? EndTime { get; set; }

        [StringLength(50)]
        public string EventStatus { get; set; }
    }
}
