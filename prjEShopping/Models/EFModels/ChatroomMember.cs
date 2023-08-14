namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ChatroomMember
    {
        [Key]
        public int ChatroomId { get; set; }

        public int? UserId { get; set; }

        public int? SellerId { get; set; }
    }
}
