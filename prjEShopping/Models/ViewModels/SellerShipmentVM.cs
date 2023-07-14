using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class SellerShipmentVM
    {
        [DisplayName("出貨單號")]
        public string ShipmentNumber { get; set; }

        [DisplayName("訂單日期")]
        public DateTime ShipmentDate { get; set; }

        [DisplayName("出貨狀態")]
        public string ShipmentStatus { get; set; }
    }
}