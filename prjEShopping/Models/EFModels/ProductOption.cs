namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductOption
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OptionId { get; set; }

        [StringLength(50)]
        public string OptionName { get; set; }

        public int? SpecificationId { get; set; }
    }
}
