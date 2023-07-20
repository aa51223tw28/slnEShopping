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
