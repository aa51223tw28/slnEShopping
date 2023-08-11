using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
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

    public partial class SupportVM
    {
        public int SupportId { get; set; }

        [StringLength(50)]
        public string SupportNumber { get; set; }

        public int? AdminId { get; set; }

        public int? SellerId { get; set; }

        public int? UserId { get; set; }

        public int? ProductId { get; set; }

        [StringLength(50)]
        public string SupportType { get; set; }

        [StringLength(50)]
        public string SupportTitle { get; set; }

        [StringLength(4000)]
        public string SupportText { get; set; }

        public DateTime? ReceivedTime { get; set; }

        [StringLength(50)]
        public string SupportStatus { get; set; }

        [StringLength(100)]
        public string ImageLink { get; set; }
    }

    public partial class SupportReplayVM
    {
        public int SupportReplayId { get; set; }

        public int? SupportId { get; set; }

        public int? AdminId { get; set; }

        public int? SellerId { get; set; }

        public int? UserId { get; set; }

        [StringLength(50)]
        public string ReplayTitle { get; set; }

        [StringLength(4000)]
        public string ReplayText { get; set; }

        public DateTime? ReplayTime { get; set; }

        [StringLength(50)]
        public string SupportReplayStatus { get; set; }

        [StringLength(100)]
        public string ImageLink { get; set; }
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
        }
    }