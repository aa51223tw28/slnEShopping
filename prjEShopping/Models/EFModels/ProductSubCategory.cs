namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductSubCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductSubCategory()
        {
            Products = new HashSet<Product>();
            ProductSpecifications = new HashSet<ProductSpecification>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SubcategoryId { get; set; }

        [StringLength(50)]
        public string SubcategoryName { get; set; }

        public int? CategoryId { get; set; }

        public virtual ProductMainCategory ProductMainCategory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductSpecification> ProductSpecifications { get; set; }
    }
}
