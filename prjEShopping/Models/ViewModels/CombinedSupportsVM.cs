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
    }

    public partial class SupportReplayVM
    {
        public int SupportReplayId { get; set; }

        public int? SupportId { get; set; }

        public int? AdminId { get; set; }

        [StringLength(50)]
        public string ReplayTitle { get; set; }

        [StringLength(4000)]
        public string ReplayText { get; set; }

        public DateTime? ReplayTime { get; set; }

        [StringLength(50)]
        public string SupportReplayStatus { get; set; }
    }
}