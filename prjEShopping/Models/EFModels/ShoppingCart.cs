namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ShoppingCart
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ShoppingCart()
        {
            ShoppingCartDetails = new HashSet<ShoppingCartDetail>();
        }

        [Key]
        public int CartId { get; set; }

        public int? UserId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShoppingCartDetail> ShoppingCartDetails { get; set; }

        public virtual User User { get; set; }
    }
}
