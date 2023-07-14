using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class SellerShipmentVM
    {
        public int ShipmentNumber { get; set; }

        public DateTime ShipmentDate { get; set; }

        public int ShipmentStatusId { get; set; }
    }
}