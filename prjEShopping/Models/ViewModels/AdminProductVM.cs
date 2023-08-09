using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using prjEShopping.Models.EFModels;
using System.ComponentModel;
using System.Security;

namespace prjEShopping.Models.ViewModels
{
    public class AdminProductVM
    {
        [DisplayName("商品ID")]
        public int ProductId { get; set; }

        public int? SellerId { get; set; }

        [DisplayName("品名")]
        [StringLength(50)]
        public string ProductName { get; set; }

        [DisplayName("商品說明")]
        [StringLength(4000)]
        public string ProductDescription { get; set; }

        [DisplayName("商品價格")]
        [Column(TypeName = "money")]
        public decimal? Price { get; set; }

        [DisplayName("審核狀態")]
        public int? ProductStatusId { get; set; }
        public string ProductStatusName
        {
            get
            {
                switch (ProductStatusId)
                {
                    case 1:
                        return "審核中";
                    case 2:
                        return "販售中";
                    case 3:
                        return "下架中";
                   default:
                        return "";
                }
            }
        }

        [DisplayName("品牌")]
        public int? BrandId { get; set; }

        [DisplayName("商品預覽")]
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
    }

    public static class AdminProductChange
    {
        public static AdminProductVM AdminProduct2VM(Product p)
        {
            return new AdminProductVM
            {
                ProductId = p.ProductId,
                SellerId = p.SellerId,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                Price = p.Price,
                ProductStatusId = p.ProductStatusId,
                BrandId =p.BrandId,
                ProductImagePathOne = p.ProductImagePathOne,
                ProductImagePathTwo = p.ProductImagePathTwo,
                ProductImagePathThree = p.ProductImagePathThree,
                CategoryId = p.CategoryId,
                SubcategoryId = p.SubcategoryId,
                OptionIdOne = p.OptionIdOne,
                OptionIdTwo = p.OptionIdTwo,
                OptionIdThree = p.OptionIdThree,
                OptionIdFour = p.OptionIdFour,
                OptionIdFive = p.OptionIdFive,
                Promote = p.Promote,
            };
        }

        public static List<AdminProductVM> AdminProduct2VM(this IEnumerable<Product> source)
        {
            if (source == null || source.Count() == 0)
            {
                return Enumerable.Empty<AdminProductVM>().ToList();
            }

            return source.Select(p => AdminProduct2VM(p)).ToList();
        }

        public static Product VM2AdminProduct(AdminProductVM p)
        {
            return new Product
            {
                ProductId = p.ProductId,
                SellerId = p.SellerId,
                ProductName = p.ProductName,
                ProductDescription = p.ProductDescription,
                Price = p.Price,
                ProductStatusId = p.ProductStatusId,
                BrandId = p.BrandId,
                ProductImagePathOne = p.ProductImagePathOne,
                ProductImagePathTwo = p.ProductImagePathTwo,
                ProductImagePathThree = p.ProductImagePathThree,
                CategoryId = p.CategoryId,
                SubcategoryId = p.SubcategoryId,
                OptionIdOne = p.OptionIdOne,
                OptionIdTwo = p.OptionIdTwo,
                OptionIdThree = p.OptionIdThree,
                OptionIdFour = p.OptionIdFour,
                OptionIdFive = p.OptionIdFive,
                Promote = p.Promote,
            };
        }
    }
}