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

        [Display(Name ="�鸹")]
        public string CouponNumber { get; set; }

        [Display(Name = "��W")]
        [StringLength(50)]
        public string CouponName { get; set; }

        [Display(Name = "����")]
        [StringLength(50)]
        public string CouponDetails { get; set; }

        [Display(Name = "�ƶq")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "�ƶq�����O�Ʀr")]
        public int? Quantity { get; set; }

        [Display(Name = "�ϥα���")]
        [StringLength(50)]
        public string CouponTerms { get; set; }

        [Display(Name = "�馩���O")]
        [StringLength(50)]
        public string CouponType { get; set; }

        [Display(Name = "�馩���e")]
        [StringLength(50)]
        public string Discount { get; set; }

        [Display(Name = "�ϥνd��")]
        [StringLength(50)]
        public string Storewide { get; set; }

        [Display(Name = "�}�l�ɶ�")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartTime { get; set; }

        [Display(Name = "����I��")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ClaimDeadline { get; set; }

        [Display(Name = "���ʺI��")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndTime { get; set; }

        [Display(Name = "���ʪ��A")]
        [StringLength(50)]
        public string EventStatus { get; set; }
    }
}
