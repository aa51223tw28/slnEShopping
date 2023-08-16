using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class SellerRegisterVM
    {
        [DisplayName("負責人姓名")]
        public string SellerName { get; set; }
       
        [DisplayName("商家姓名")]
        public string StoreName { get; set; }

        [DisplayName("商家編號")]
        public int? SellerId { get; set; }

        [DisplayName("使用者帳號(電子郵件)")]
        [StringLength(50)]
        public string SellerAccount { get; set; }

        [DisplayName("使用者密碼")]
        [DataType(DataType.Password)]
        [StringLength(50)]
        public string SellerPassword { get; set; }

        [DisplayName("手機號碼")]
        [StringLength(50)]
        public string Phone { get; set; }

        [DisplayName("地址")]
        [StringLength(50)]
        public string Address { get; set; }



        [DisplayName("商家統編")]
        public string GUINumber { get; set; }
       
        [DisplayName("商店介紹")]
        [StringLength(4000)]
        public string StoreIntro { get; set; }
        
        [DisplayName("訪問權")]
        public int? AccessRightId { get; set; }
        
        [DisplayName("銀行帳戶")]
        public string BankAccount { get; set; }
       
        [DisplayName("支付方式")]
        public int? PaymentMethodId { get; set; }
       
        [DisplayName("取貨方式")]
        public int? ShippingMethodId { get; set; }
       
        [DisplayName("紅利點數")]
        public int? ADPoints { get; set; }
       
        [DisplayName("商家圖片")]
        public HttpPostedFileBase SellerImagePath { get; set; }
        public HttpPostedFileBase photo { get; set; }
    }
}