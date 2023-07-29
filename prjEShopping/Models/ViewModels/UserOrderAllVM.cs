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
        public string SellerImagePath { get; set; }
        public int Quantity { get; set; }       //這數量為分賣家加總的數量
        public decimal PriceBySeller { get; set; }
        public int ShipmentStatusId { get; set; }
        public string ShipmentStatus { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int QuantityByProduct { get; set; }//一張訂單有購買的數量
        public decimal CurrentPrice { get; set; }
        public decimal SubTotal { get; set; }
        public string ProductImagePathOne { get; set; }
    }
}