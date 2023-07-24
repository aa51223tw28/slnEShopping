using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class UserShoppingCartVM
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int CartDetailId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int SellerId { get; set; }
        //public string SellerName { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Price { get; set; }

        public string ProductImagePathOne { get; set; }
    }
}

