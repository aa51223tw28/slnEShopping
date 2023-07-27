using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.DTOs
{
    public class UserProductIndexDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }       
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        public string ProductImagePathOne { get; set; }
        public string ProductImagePathTwo { get; set; }
        public string ProductImagePathThree { get; set; }
        public int ProductStock { get; set; }
        public int QuantitySold { get; set; }
        //待確認還有其他大分類小分類規格...
        public string CategoryName { get; set; }
        
        

    }
}