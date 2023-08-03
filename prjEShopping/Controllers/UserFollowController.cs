using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class UserFollowController : Controller
    {
        // GET: UserFollow
        [Authorize]
        public ActionResult UserFollowSeller()
        {
            return View();
        }


        [Authorize]
        public ActionResult UserFollowProduct()
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();

            List<UserFollowProductVM> datas= new List<UserFollowProductVM>();

            var trackproduct = db.TrackProducts.Where(x => x.UserId == userid)
                                                .Join(db.Products, x => x.ProductId, y => y.ProductId, (x, y) => new
                                                {
                                                    ProductId=x.ProductId,
                                                    ProductName=y.ProductName,
                                                    Price=y.Price,
                                                    ProductImagePathOne=y.ProductImagePathOne,
                                                    TrackProductId=x.TrackProductId,
                                                }).ToList();

            datas= trackproduct.Select(x=>new UserFollowProductVM
            {
                ProductId=(int)x.ProductId,
                ProductName=x.ProductName,
                Price=(decimal)x.Price,
                ProductImagePathOne=x.ProductImagePathOne,
                TrackProductId=x.TrackProductId,
            }).ToList();


            return View(datas);
        }
    }
}