namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Message
    {
        public int MessageId { get; set; }

        public int? ChatroomId { get; set; }

        [StringLength(4000)]
        public string SenderId { get; set; }

        [StringLength(4000)]
        public string Text { get; set; }

        public DateTime? Timestamp { get; set; }
    }
}
