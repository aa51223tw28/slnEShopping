namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductSubCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SubcategoryId { get; set; }

        [StringLength(50)]
        public string SubcategoryName { get; set; }

        public int? CategoryId { get; set; }
    }
}
