namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ShoppingCartDetail
    {
        [Key]
        public int CartDetailId { get; set; }

        public int? CartId { get; set; }

        public int? ProductId { get; set; }

        public int? Quantity { get; set; }
    }
}
