namespace prjEShopping.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Seller
    {
        public int SellerId { get; set; }

        [StringLength(50)]
        public string SellerName { get; set; }

        [StringLength(50)]
        public string StoreName { get; set; }

        [StringLength(50)]
        public string SellerAccount { get; set; }

        [StringLength(100)]
        public string SellerPassword { get; set; }

        [StringLength(100)]
        public string SellerPasswordSalt { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string GUINumber { get; set; }

        [StringLength(4000)]
        public string StoreIntro { get; set; }

        public int? AccessRightId { get; set; }

        [StringLength(50)]
        public string BankAccount { get; set; }

        public int? PaymentMethodId { get; set; }

        public int? ShippingMethodId { get; set; }

        public int? ADPoints { get; set; }

        [StringLength(50)]
        public string SellerImagePath { get; set; }

        [StringLength(50)]
        public string Role { get; set; }

        [StringLength(100)]
        public string EmailCheck { get; set; }
    }
}
