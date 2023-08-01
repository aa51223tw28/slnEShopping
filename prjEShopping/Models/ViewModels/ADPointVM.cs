using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class ADPointVM
    {
        
            public int ADPointId { get; set; }

            [DisplayName("商家Id")]
            public int? SellerId { get; set; }

            [DisplayName("商家統編")]
            [StringLength(50)]
            public string GUINumber { get; set; }

             [DisplayName("購買時間")]
             [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
             public DateTime? PurchaseTime { get; set; }

            [DisplayName("廣告點數")]
            public int? ADPoints { get; set; }

            [DisplayName("廣告點數")]
            [StringLength(50)]
            public string PaymentStatus { get; set; }
       
    }

    public static class ADPointChange
    {
        public static ADPoint VM2ADPoint(ADPointVM vm)
        {          
            return new ADPoint
            {
                ADPointId = vm.ADPointId,
                SellerId = vm.SellerId,
                GUINumber = vm.GUINumber,
                PurchaseTime = vm.PurchaseTime,
                ADPoints = vm.ADPoints,
                PaymentStatus = vm.PaymentStatus,
            };
        }

        public static ADPointVM ADPoint2VM(ADPoint point)
        {
            var db = new AppDbContext();
            var seller=db.Sellers.Where(x=>x.SellerId==point.SellerId).FirstOrDefault();
            return new ADPointVM
            {
                ADPointId = point.ADPointId,
                SellerId = seller.SellerId,
                GUINumber = seller.GUINumber,
                PurchaseTime = point.PurchaseTime,
                ADPoints = point.ADPoints,
                PaymentStatus = point.PaymentStatus,
            };
        }

        public static List<ADPointVM> ADPoint2VM(this IEnumerable<ADPoint> point)
        {
            if (point == null || point.Count() == 0)
            {
                return Enumerable.Empty<ADPointVM>().ToList();
            }

            return point.Select(p => ADPoint2VM(p)).ToList();
        }
    }
}