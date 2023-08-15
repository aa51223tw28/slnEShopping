namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UsersCoupon
    {
        [Key]
        [Column(Order = 0)]
        public int UsersCouponId { get; set; }

        public int? UserId { get; set; }

        //[Key]
        //[Column(Order = 1)]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CouponId { get; set; }

        public DateTime? GetDate { get; set; }

        public DateTime? UseDate { get; set; }

        [StringLength(50)]
        public string CouponStatus { get; set; }

        public virtual User User { get; set; }
    }
}
