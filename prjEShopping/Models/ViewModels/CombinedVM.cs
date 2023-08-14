using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class CombinedVM
    {
        public ProductViewModel ProductViewModel { get; set; }
        public ShipmentViewModel ShipmentViewModel { get; set; }
    }

    public class ShipmentViewModel
    {
        public int ShipmentDetailId { get; set; }
        public string ShipmentNumber { get; set; }
    }

    public class ProductViewModel
    {
        public string ProductName { get; set; }
        public int? ProductId { get; set; }
    }
}