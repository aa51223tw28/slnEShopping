namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ShippingMethod
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ShippingMethod()
        {
            Sellers = new HashSet<Seller>();
            Users = new HashSet<User>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ShippingMethodId { get; set; }

        [StringLength(50)]
        public string ShippingMethodName { get; set; }

        [Column(TypeName = "money")]
        public decimal? Freight { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Seller> Sellers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
    }
}
