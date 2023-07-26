using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace prjEShopping.Models.DTOs
{
    public class CouponDto
    {
        public int CouponId { get; set; }

        public int? SellerId { get; set; }

        public string CouponNumber { get; set; }

        public string CouponName { get; set; }

        public string CouponDetails { get; set; }

        public int? Quantity { get; set; }

        public int? ReceivedQuantity { get; set; }

        public string CouponTerms { get; set; }

        public string CouponType { get; set; }

        public string Discount { get; set; }

        public string Storewide { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? ClaimDeadline { get; set; }

        public DateTime? EndTime { get; set; }

        public string EventStatus { get; set; }
    }
}