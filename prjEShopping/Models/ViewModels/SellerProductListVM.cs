using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class SellerProductListVM
    {
        public int ProductID { get; set; }
        [DisplayName("商品名稱")]
        public string ProductName { get; set; }
        [DisplayName("價格")]
        public int Price { get; set; }
        [DisplayName("商品圖片")]
        public string ProductImagePathOne { get; set; }
        [DisplayName("庫存")]
        public int? StockQuantity { get; set; }
        [DisplayName("訂單內數量")]
        public int? OrderQuantity { get; set; }
        [DisplayName("可購買數量")]
        public int? CanBuyQuantity 
        {
            get { return (this.StockQuantity - this.OrderQuantity); } 
        }
        [DisplayName("商品狀態")]
        public string ProductStatusName { get; set; }

        public string Promote { get; set; }
    }
}