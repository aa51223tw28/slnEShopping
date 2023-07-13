namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ShipmentDetail
    {
        public int ShipmentDetailId { get; set; }

        [StringLength(50)]
        public string ShipmentNumber { get; set; }

        public int? PaymentMethodId { get; set; }

        public int? ShippingMethodId { get; set; }

        [StringLength(50)]
        public string Receiver { get; set; }

        [StringLength(50)]
        public string ReceiverAddress { get; set; }
    }
}
