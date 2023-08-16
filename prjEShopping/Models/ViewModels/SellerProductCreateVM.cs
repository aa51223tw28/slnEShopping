using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class SellerProductCreateVM
    {
        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }


        public decimal? Price { get; set; }

        public string BrandName { get; set; }

        public string ProductImagePathOne { get; set; }
        public HttpPostedFileBase[] photo1 { get; set; }

        public string ProductImagePathTwo { get; set; }
        //public HttpPostedFileBase photo2 { get; set; }

        public string ProductImagePathThree { get; set; }
        //public HttpPostedFileBase photo3 { get; set; }

        public int? Quantity { get; set; }

        public string CategoryName { get; set; }

        public string SubcategoryName { get; set; }

        public string SpecificationName0 { get; set; }

        public string SpecificationName1 { get; set; }

        public string SpecificationName2 { get; set; }

        public string SpecificationName3 { get; set; }

        public string SpecificationName4 { get; set; }

        public string OptionName0 { get; set; }

        public string OptionName1 { get; set; }

        public string OptionName2 { get; set; }

        public string OptionName3 { get; set; }

        public string OptionName4 { get; set; }
    }
}