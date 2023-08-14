namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductSpecification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SpecificationId { get; set; }

        [StringLength(50)]
        public string SpecificationName { get; set; }

        public int? SubcategoryId { get; set; }

        public virtual ProductSubCategory ProductSubCategory { get; set; }
    }
}
