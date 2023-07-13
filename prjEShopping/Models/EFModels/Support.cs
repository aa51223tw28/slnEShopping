namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Support
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
}
