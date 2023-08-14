namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TrackProduct
    {
        public int TrackProductId { get; set; }

        public int? UserId { get; set; }

        public int? ProductId { get; set; }

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
}
