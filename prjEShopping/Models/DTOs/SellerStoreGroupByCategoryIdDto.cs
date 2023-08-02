using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.DTOs
{
    public class SellerStoreGroupByCategoryIdDto
    {
        public int SubcategoryId { get; set; }
        public string SubcateName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}