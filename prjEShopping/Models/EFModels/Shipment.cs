namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Shipment
    {
        public int ShipmentId { get; set; }

        [StringLength(50)]
        public string ShipmentNumber { get; set; }

        public int? OrderId { get; set; }

        public int? SellerId { get; set; }

        public DateTime? ShipmentDate { get; set; }

        public DateTime? ShipDate { get; set; }

        public DateTime? ArrivalTimeDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public DateTime? PickDate { get; set; }

        public int? ShipmentStatusId { get; set; }
    }
}
