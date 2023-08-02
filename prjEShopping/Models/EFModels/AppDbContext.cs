using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace prjEShopping.Models.EFModels
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
            : base("name=AppDbContext")
        {
        }

        public virtual DbSet<AccessRight> AccessRights { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<ADPoint> ADPoints { get; set; }
        public virtual DbSet<ADProduct> ADProducts { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Coupon> Coupons { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<ProductInventory> ProductInventories { get; set; }
        public virtual DbSet<ProductMainCategory> ProductMainCategories { get; set; }
        public virtual DbSet<ProductOption> ProductOptions { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductSpecification> ProductSpecifications { get; set; }
        public virtual DbSet<ProductStatusDetail> ProductStatusDetails { get; set; }
        public virtual DbSet<ProductStock> ProductStocks { get; set; }
        public virtual DbSet<ProductSubCategory> ProductSubCategories { get; set; }
        public virtual DbSet<RatingReplay> RatingReplaies { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Seller> Sellers { get; set; }
        public virtual DbSet<SellersAD> SellersADs { get; set; }
        public virtual DbSet<ShipmentDetail> ShipmentDetails { get; set; }
        public virtual DbSet<Shipment> Shipments { get; set; }
        public virtual DbSet<ShipmentStatusDetail> ShipmentStatusDetails { get; set; }
        public virtual DbSet<ShippingMethod> ShippingMethods { get; set; }
        public virtual DbSet<ShoppingCartDetail> ShoppingCartDetails { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<SupportReplay> SupportReplaies { get; set; }
        public virtual DbSet<Support> Supports { get; set; }
        public virtual DbSet<TrackProduct> TrackProducts { get; set; }
        public virtual DbSet<TrackSeller> TrackSellers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UsersCoupon> UsersCoupons { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.CurrentPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Product>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ShippingMethod>()
                .Property(e => e.Freight)
                .HasPrecision(19, 4);
        }
    }
}
