namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ADProduct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ADProduct()
        {
            SellersADs = new HashSet<SellersAD>();
        }

        public int ADProductId { get; set; }

        [StringLength(50)]
        public string ADName { get; set; }

        public int? ADPoint { get; set; }

        public DateTime? ADStartDate { get; set; }

        public DateTime? ADEndDate { get; set; }

        public int? ADField { get; set; }

        [StringLength(50)]
        public string ADImagePath { get; set; }

        public int? ProductId { get; set; }

        public int? Discount { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SellersAD> SellersADs { get; set; }
    }
}
