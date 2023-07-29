using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class UserShipmentDetailVM
    {
        public int ShipmentNumber { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethodName { get; set; }
        public int ShippingMethodId { get; set; }
        public string ShippingMethodName { get; set; }
        public string Receiver { get; set; }
        public string ReceiverAddress { get; set; }
    }
}