using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class ADProductVM
    {
        [DisplayName("廣告ID")]
        public int ADProductId { get; set; }

        [DisplayName("廣告名稱")]
        [StringLength(50)]
        public string ADName { get; set; }

        [DisplayName("點數")]
        public int? ADPoint { get; set; }

        [DisplayName("開始時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ADStartDate { get; set; }

        [DisplayName("結束時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ADEndDate { get; set; }

        [DisplayName("廣告欄位")]
        public int? ADQuantity { get; set; }

        [DisplayName("廣告圖片")]
        [StringLength(50)]
        public string ADImagePath { get; set; }

        [DisplayName("商品ID")]
        public int? ProductId { get; set; }
    }

    public static class ADProductChange
    {
        public static ADProductVM ADProduct2VM(ADProduct ad)
        {
            return new ADProductVM
            {
                ADProductId = ad.ADProductId,
                ADName = ad.ADName,
                ADPoint = ad.ADPoint,
                ADStartDate = ad.ADStartDate,
                ADEndDate = ad.ADEndDate,
                ADQuantity = ad.ADQuantity,
                ADImagePath = ad.ADImagePath,
                ProductId = ad.ProductId,
            };
        }

        public static ADProduct VM2ADProduct(ADProductVM vm)
        {
            return new ADProduct
            {
                ADProductId = vm.ADProductId,
                ADName = vm.ADName,
                ADPoint =vm.ADPoint,
                ADStartDate = vm.ADStartDate,
                ADEndDate = vm.ADEndDate,
                ADQuantity = vm.ADQuantity,
                ADImagePath = vm.ADImagePath,
                ProductId = vm.ProductId,
            };
        }


        public static List<ADProductVM> ADProduct2VM(this IEnumerable<ADProduct> source)
        {
            if (source == null || source.Count() == 0)
            {
                return Enumerable.Empty<ADProductVM>().ToList();
            }

            return source.Select(s => ADProduct2VM(s)).ToList();
        }
    }
}