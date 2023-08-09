using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class AdminSellerVM
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

    public static class AdminSellerChange
    {
        public static AdminSellerVM AdminSeller2VM(Seller s)
        {
            return new AdminSellerVM
            {
                SellerId=s.SellerId,
                SellerName=s.SellerName,
                StoreName=s.StoreName,
                SellerAccount=s.SellerAccount,
                SellerPassword=s.SellerPassword,
                SellerPasswordSalt=s.SellerPasswordSalt,
                Phone=s.Phone,
                Address=s.Address,
                GUINumber=s.GUINumber,
                StoreIntro=s.StoreIntro,
                AccessRightId=s.AccessRightId,
                BankAccount=s.BankAccount,
                PaymentMethodId=s.PaymentMethodId,
                ShippingMethodId=s.ShippingMethodId,
                ADPoints=s.ADPoints,
                SellerImagePath=s.SellerImagePath,
                Role=s.Role,
                EmailCheck=s.EmailCheck,
            };
        }

        public static List<AdminSellerVM> AdminSeller2VM(this IEnumerable<Seller> source)
        {
            if (source == null || source.Count() == 0)
            {
                return Enumerable.Empty<AdminSellerVM>().ToList();
            }

            return source.Select(s => AdminSeller2VM(s)).ToList();
        }
    }
}