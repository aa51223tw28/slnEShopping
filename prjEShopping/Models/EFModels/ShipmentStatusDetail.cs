namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ShipmentStatusDetail
    {
        [Key]
        public int ShipmentStatusId { get; set; }

        [StringLength(50)]
        public string ShipmentStatus { get; set; }
    }
}
