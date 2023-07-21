using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
    public class UserShoppingCartVM
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public IEnumerable<UserShoppingCartItemVM> CartItems { get; set; }
        public decimal Total
        {
            get
            {
                return CartItems.Sum(x => x.SubTotal);
            }
        }

        public bool AllowCheckout=>CartItems.Any();//any表示如果購物車有一個東西就允許結帳
    }


    public class UserShoppingCartItemVM
    {
        public int CartDetailId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal SubTotal => (decimal)(Product.Price * Quantity);
    }
}