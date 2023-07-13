namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductId { get; set; }

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
