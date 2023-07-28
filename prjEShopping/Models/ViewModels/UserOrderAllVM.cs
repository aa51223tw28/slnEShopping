using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class UserOrderAllVM
    {              
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public int ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public int SellerId { get; set; }
        public string SellerName { get; set; }
        public int Quantity { get; set; }        
        public string SellerImagePath { get; set; }
        public decimal PriceBySeller { get; set; }
        public int ShipmentStatusId { get; set; }
    }
}