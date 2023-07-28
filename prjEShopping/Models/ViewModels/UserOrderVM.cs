using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class UserOrderVM//長的跟UserShoppingCartVM很像待之後優化
    {

        //public int OrderId { get; set; }
        //public int UserId { get; set; }
        //public int CouponId { get; set; }//先寫死
        //public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }
        //public int OrderDetailId { get; set; }
        //public int ProductId { get; set; }
        public string ProductName { get; set; }
        //public int SellerId { get; set; }
        //public string SellerName { get; set; }
        public int Quantity { get; set; }
        //public decimal CurrentPrice { get; set; }
        public decimal CurrentSubTotal { get; set; }
        public string ProductImagePathOne { get; set; }

    }
}