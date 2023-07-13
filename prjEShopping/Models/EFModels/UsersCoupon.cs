namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UsersCoupon
    {
        public int UsersCouponId { get; set; }

        public int? UserId { get; set; }

        public int? CouponId { get; set; }

        public DateTime? GetDate { get; set; }

        public DateTime? UseDate { get; set; }

        [StringLength(50)]
        public string CouponStatus { get; set; }
    }
}
