﻿using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class UserCartController : Controller
    {
        // GET: UserCart

        [Authorize]
        public ActionResult UserAddCart(int ProductId)//傳過來買了什麼的api
        {
            var db = new AppDbContext();
            var customerAccount = User.Identity.Name;

            //找userid
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x=>x.UserId).FirstOrDefault();

            //新增一台購物車
            var cart = db.ShoppingCarts.FirstOrDefault(x => x.UserId == userid);
            //如果沒有購物車直接給他一台車
            if (cart == null)
            {
                var shoppingcart = new ShoppingCart()
                {
                    UserId = userid
                };
                db.ShoppingCarts.Add(shoppingcart);
                db.SaveChanges();
            }

            //取得新增資料加入table
            var cartid = db.ShoppingCarts.Where(x => x.UserId == userid).OrderByDescending(x=>x.CartId).Select(x => x.CartId).FirstOrDefault();

            //List<int> qua = new List<int>();
            //var shoppingdetails = db.ShoppingCartDetails.Where(x => x.CartId == cartid && x.ProductId == ProductId).Select(x => x.Quantity);
            //foreach (var item in shoppingdetails)
            //{
            //    qua.Add((int)item);
            //}
            ////加入ShoppingCartDetails table
            //ShoppingCartDetail shoppingCartDetail;

            //確認是否已經加入過同一種商品
            var shoppingCartDetail = db.ShoppingCartDetails.FirstOrDefault(x => x.CartId == cartid && x.ProductId == ProductId);
            if (shoppingCartDetail == null)//沒加過
            {
                shoppingCartDetail = new ShoppingCartDetail()
                {
                    CartId= cartid,
                    ProductId= ProductId,
                    Quantity=1//先預設1
                };
                db.ShoppingCartDetails.Add(shoppingCartDetail);
                
            }
            else
            {
                shoppingCartDetail.Quantity += 1;
            }
            db.SaveChanges();
            return new EmptyResult();//這個Action方法返回一個空結果(EmptyResult)，表示操作已經完成，並不需要返回任何特定的內容或視圖。
        }


        [Authorize]
        public ActionResult UserShoppingCart()//購物車頁面
        {
            return View();
        }


        [Authorize]
        public ActionResult UserCheckout()//結帳頁面
        {
            return View();
        }
        
        [Authorize]
        public ActionResult UserOrderDetail()//訂單詳情頁面
        {
            return View();
        }
    }
 }
