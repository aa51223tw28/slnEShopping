namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            ProductInventories = new HashSet<ProductInventory>();
            ProductStocks = new HashSet<ProductStock>();
            Ratings = new HashSet<Rating>();
            ShoppingCartDetails = new HashSet<ShoppingCartDetail>();
            TrackProducts = new HashSet<TrackProduct>();
        }

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

        public int? Promote { get; set; }

        public virtual Brand Brand { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductInventory> ProductInventories { get; set; }

        public virtual ProductMainCategory ProductMainCategory { get; set; }

        public virtual ProductStatusDetail ProductStatusDetail { get; set; }

        public virtual ProductSubCategory ProductSubCategory { get; set; }

        public virtual Seller Seller { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductStock> ProductStocks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rating> Ratings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShoppingCartDetail> ShoppingCartDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TrackProduct> TrackProducts { get; set; }
    }
}
