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

        [StringLength(50)]

        [Display(Name ="券號")]
        public string CouponNumber { get; set; }

        [Display(Name = "券名")]
        [StringLength(50)]
        public string CouponName { get; set; }

        [Display(Name = "說明")]
        [StringLength(50)]
        public string CouponDetails { get; set; }

        [Display(Name = "數量")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "數量必須是數字")]
        public int? Quantity { get; set; }

        [Display(Name = "使用條件")]
        [StringLength(50)]
        public string CouponTerms { get; set; }

        [Display(Name = "折扣類別")]
        [StringLength(50)]
        public string CouponType { get; set; }

        [Display(Name = "折扣內容")]
        [StringLength(50)]
        public string Discount { get; set; }

        [Display(Name = "使用範圍")]
        [StringLength(50)]
        public string Storewide { get; set; }

        [Display(Name = "開始時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartTime { get; set; }

        [Display(Name = "領取截止")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ClaimDeadline { get; set; }

        [Display(Name = "活動截止")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndTime { get; set; }

        [Display(Name = "活動狀態")]
        [StringLength(50)]
        public string EventStatus { get; set; }
    }
}
