namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {
        public int ProductId { get; set; }

        public int? SellerId { get; set; }

        [StringLength(50)]
        public string ProductName { get; set; }

        [StringLength(4000)]
        public string ProductDescription { get; set; }

        [Column(TypeName = "money")]
        public decimal? Price { get; set; }

        public int? ProductStatusId { get; set; }

        public int? BrandId { get; set; }

        [StringLength(50)]
        public string ProductImagePathOne { get; set; }

        [StringLength(50)]
        public string ProductImagePathTwo { get; set; }

        [StringLength(50)]
        public string ProductImagePathThree { get; set; }

        public int? CategoryId { get; set; }

        public int? SubcategoryId { get; set; }

        [StringLength(50)]
        public string OptionIdOne { get; set; }

        [StringLength(50)]
        public string OptionIdTwo { get; set; }

        [StringLength(50)]
        public string OptionIdThree { get; set; }

        [StringLength(50)]
        public string OptionIdFour { get; set; }

        [StringLength(50)]
        public string OptionIdFive { get; set; }
    }
}
