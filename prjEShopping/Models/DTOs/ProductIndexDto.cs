﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.DTOs
{
    public class ProductIndexDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }//待確認
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        public string ProductImagePathOne { get; set; }
        public string ProductImagePathTwo { get; set; }
        public string ProductImagePathThree { get; set; }
        public int Stock { get; set; }
    }
}