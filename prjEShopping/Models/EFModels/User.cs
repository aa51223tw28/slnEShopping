namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Orders = new HashSet<Order>();
            ShoppingCarts = new HashSet<ShoppingCart>();
            TrackProducts = new HashSet<TrackProduct>();
            TrackSellers = new HashSet<TrackSeller>();
            UsersCoupons = new HashSet<UsersCoupon>();
        }

        public int UserId { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string UserAccount { get; set; }

        [StringLength(100)]
        public string UserPassword { get; set; }

        [StringLength(100)]
        public string UserPasswordSalt { get; set; }

        [StringLength(50)]
        public string Gender { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string CellPhone { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        public int? ShippingMethodId { get; set; }

        public int? PaymenyMethodId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Birthday { get; set; }

        public int? AccessRightId { get; set; }

        [StringLength(50)]
        public string UserImagePath { get; set; }

        [StringLength(50)]
        public string Role { get; set; }

        [StringLength(100)]
        public string EmailCheck { get; set; }

        public virtual AccessRight AccessRight { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }

        public virtual ShippingMethod ShippingMethod { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TrackProduct> TrackProducts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TrackSeller> TrackSellers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsersCoupon> UsersCoupons { get; set; }
    }
}
