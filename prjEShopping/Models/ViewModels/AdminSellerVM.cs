using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class AdminSellerVM
    {
        [DisplayName("商家ID")]
        public int SellerId { get; set; }

        [DisplayName("負責人")]
        [StringLength(50)]
        public string SellerName { get; set; }

        [DisplayName("商家名")]
        [StringLength(50)]
        public string StoreName { get; set; }

        [DisplayName("商家帳號")]
        [StringLength(50)]
        public string SellerAccount { get; set; }

        [DisplayName("商家密碼")]
        [StringLength(100)]
        public string SellerPassword { get; set; }

        [StringLength(100)]
        public string SellerPasswordSalt { get; set; }

        [DisplayName("電話")]
        [StringLength(50)]
        public string Phone { get; set; }

        [DisplayName("地址")]
        [StringLength(50)]
        public string Address { get; set; }

        [DisplayName("統編")]
        [StringLength(50)]
        public string GUINumber { get; set; }

        [DisplayName("商家介紹")]
        [StringLength(4000)]
        public string StoreIntro { get; set; }

        [DisplayName("權限")]
        public int? AccessRightId { get; set; }

        [DisplayName("銀行帳戶")]
        [StringLength(50)]
        public string BankAccount { get; set; }

        [DisplayName("付款方式")]
        public int? PaymentMethodId { get; set; }

        [DisplayName("貨運方式")]
        public int? ShippingMethodId { get; set; }

        [DisplayName("廣告點數")]
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