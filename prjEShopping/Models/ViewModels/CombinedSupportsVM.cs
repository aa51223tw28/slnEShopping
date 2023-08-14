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
    public class CombinedSupportsVM
    {
        public Support Support { get; set; }
        public SupportReplay SupportReplay { get; set; }
    }

    public class SupportDetailViewModel
    {
        public SupportVM Support { get; set; }
        public List<SupportReplayVM> SupportReplies { get; set; }
    }

    public partial class SupportVM
    {
        public int SupportId { get; set; }

        [DisplayName("信件編號")]
        [StringLength(50)]
        public string SupportNumber { get; set; }

        [DisplayName("管理員ID")]
        public int? AdminId { get; set; }

        [DisplayName("商家ID")]
        public int? SellerId { get; set; }

        [DisplayName("會員ID")]
        public int? UserId { get; set; }

        [DisplayName("商品ID")]
        public int? ProductId { get; set; }

        [DisplayName("信件類型")]
        [StringLength(50)]
        public string SupportType { get; set; }

        [DisplayName("標題")]
        [StringLength(50)]
        public string SupportTitle { get; set; }

        [DisplayName("內容")]
        [StringLength(4000)]
        public string SupportText { get; set; }

        [DisplayName("發信時間")]
        public DateTime? ReceivedTime { get; set; }

        [DisplayName("信件狀態")]
        [StringLength(50)]
        public string SupportStatus { get; set; }

        [DisplayName("上傳圖片")]
        [StringLength(100)]
        public string ImageLink { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }
    }

    public partial class SupportReplayVM
    {
        [DisplayName("回覆ID")]
        public int SupportReplayId { get; set; }

        public int? SupportId { get; set; }

        public int? AdminId { get; set; }

        public int? SellerId { get; set; }

        public int? UserId { get; set; }

        [DisplayName("回覆標題")]
        [StringLength(50)]
        public string ReplayTitle { get; set; }

        [DisplayName("回覆內容")]
        [StringLength(4000)]
        public string ReplayText { get; set; }

        [DisplayName("回覆時間")]
        public DateTime? ReplayTime { get; set; }

        [DisplayName("回覆狀態")]
        [StringLength(50)]
        public string SupportReplayStatus { get; set; }

        [DisplayName("上傳圖片")]
        [StringLength(100)]
        public string ImageLink { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }
    }


    public static class SupportChange
    {
        public static SupportVM Support2VM(Support s)
        {
            return new SupportVM
            {
                SupportId = s.SupportId,
                SupportNumber = s.SupportNumber,
                AdminId = s.AdminId,
                SellerId = s.SellerId,
                UserId = s.UserId,
                ProductId = s.ProductId,
                SupportType = s.SupportType,
                SupportTitle = s.SupportTitle,
                SupportText = s.SupportText,
                ReceivedTime = s.ReceivedTime,
                SupportStatus = s.SupportStatus,
                ImageLink = s.ImageLink,
            };
        }
        public static List<SupportVM> Support2VM(this IEnumerable<Support> source)
        {
            if (source == null || source.Count() == 0)
            {
                return Enumerable.Empty<SupportVM>().ToList();
            }
            return source.Select(s => Support2VM(s)).ToList();
        }
        public static Support VM2Support(SupportVM vm)
        {
            return new Support
            {
                SupportId = vm.SupportId,
                SupportNumber = vm.SupportNumber,
                AdminId = vm.AdminId,
                SellerId = vm.SellerId,
                UserId = vm.UserId,
                ProductId = vm.ProductId,
                SupportType = vm.SupportType,
                SupportTitle = vm.SupportTitle,
                SupportText = vm.SupportText,
                ReceivedTime = vm.ReceivedTime,
                SupportStatus = vm.SupportStatus,
                ImageLink = vm.ImageLink,
            };
        }
       
    }
    public static class SupportReplayChange
    {
        public static SupportReplayVM SupportReplay2VM(SupportReplay r)
        {
            return new SupportReplayVM
            {
                SupportReplayId = r.SupportReplayId,
                SupportId = r.SupportId,
                AdminId = r.AdminId,
                SellerId = r.SellerId,
                UserId = r.UserId,
                ReplayTitle = r.ReplayTitle,
                ReplayText = r.ReplayText,
                ReplayTime = r.ReplayTime,
                SupportReplayStatus = r.SupportReplayStatus,
                ImageLink = r.ImageLink,
            };
        }
        public static List<SupportReplayVM> SupportReplay2VM(this IEnumerable<SupportReplay> source)
        {
            if (source == null || source.Count() == 0)
            {
                return Enumerable.Empty<SupportReplayVM>().ToList();
            }
            return source.Select(s => SupportReplay2VM(s)).ToList();
        }
        public static SupportReplay VM2SupportReplay(SupportReplayVM vm)
        {
            return new SupportReplay
            {
                SupportReplayId = vm.SupportReplayId,
                SupportId = vm.SupportId,
                AdminId = vm.AdminId,
                SellerId = vm.SellerId,
                UserId = vm.UserId,
                ReplayTitle = vm.ReplayTitle,
                ReplayText = vm.ReplayText,
                ReplayTime = vm.ReplayTime,
                SupportReplayStatus = vm.SupportReplayStatus,
                ImageLink = vm.ImageLink,
            };
        }
    }
}