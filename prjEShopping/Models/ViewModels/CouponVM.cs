using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace prjEShopping.Models.ViewModels
{
    public class CouponVM
    {
        public int CouponId { get; set; }

        [Display(Name = "商家ID")]
        public int? SellerId { get; set; }

        [StringLength(50)]
        [Display(Name = "券號")]
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

        [Display(Name = "領取數量")]
        public int? ReceivedQuantity { get; set; }

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

    public class ImgDto
    {
       public int SellerId {get; set;}
       public string  SellerImagePath{get; set;}
    }

    public static class CouponChange
    {
        public static CouponVM Coupon2VM(Coupon c)
        {
            return new CouponVM
            {
                CouponId = c.CouponId,
                SellerId = c.SellerId,
                CouponNumber = c.CouponNumber,
                CouponName = c.CouponName,
                CouponDetails = c.CouponDetails,
                Quantity = c.Quantity,
                ReceivedQuantity = c.ReceivedQuantity,
                CouponTerms = c.CouponTerms,
                CouponType = c.CouponType,
                Discount = c.Discount,
                Storewide = c.Storewide,
                StartTime = c.StartTime,
                ClaimDeadline = c.ClaimDeadline,
                EndTime = c.EndTime,
                EventStatus = c.EventStatus
            };
        }

        public static List<CouponVM> Coupon2VM(this IEnumerable<Coupon> source)
        {
            if (source == null || source.Count() == 0)
            {
                return Enumerable.Empty<CouponVM>().ToList();
            }

            return source.Select(p => Coupon2VM(p)).ToList();
        }

        public static Coupon VM2Coupon(CouponVM c)
        {
            return new Coupon
            {
                CouponId = c.CouponId,
                SellerId = c.SellerId,
                CouponNumber = c.CouponNumber,
                CouponName = c.CouponName,
                CouponDetails = c.CouponDetails,
                Quantity = c.Quantity,
                ReceivedQuantity = c.ReceivedQuantity,
                CouponTerms = c.CouponTerms,
                CouponType = c.CouponType,
                Discount = c.Discount,
                Storewide = c.Storewide,
                StartTime = c.StartTime,
                ClaimDeadline = c.ClaimDeadline,
                EndTime = c.EndTime,
                EventStatus = c.EventStatus
            };
        }
    }

}
