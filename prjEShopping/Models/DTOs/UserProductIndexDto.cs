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



        //一堆規格        
        public int BrandId { get; set; }
        public string BrandName { get; set; }

        public string SpecificationNameOne { get; set; }
        public string SpecificationNameTwo { get; set; }
        public string SpecificationNameThree { get; set; }
        public string SpecificationNameFour { get; set; }
        public string SpecificationNameFive { get; set; }

        public string OptionNameOne { get; set; }
        public string OptionNameTwo { get; set; }
        public string OptionNameThree { get; set; }
        public string OptionNameFour { get; set; }
        public string OptionNameFive { get; set; }

        //分類
        public string CategoryName { get; set; }
        public int SubcategoryId { get; set; }//為了篩選小分類
        public string SubcategoryName { get; set; }

       
    }
}