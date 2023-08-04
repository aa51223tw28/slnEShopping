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
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();

            List<UserFollowSellerVM> datas= new List<UserFollowSellerVM>();

            var tracksellers=db.TrackSellers.Where(x=>x.UserId== userid)
                                                .Join(db.Sellers, x => x.SellerId, y => y.SellerId, (x, y) => new
                                                {
                                                    SellerId=x.SellerId,
                                                    SellerName=y.SellerName,
                                                    SellerImagePath=y.SellerImagePath,
                                                    TrackSellerId=x.TrackSellerId,
                                                }).OrderBy(x=>x.TrackSellerId).ToList();

            datas= tracksellers.Select(x=>new UserFollowSellerVM
            {
                SellerId=(int)x.SellerId,
                SellerName=x.SellerName,
                SellerImagePath=x.SellerImagePath,
                TrackSellerId=x.TrackSellerId,
            }).ToList();

            return View(datas);
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
                                                }).OrderBy(x => x.TrackProductId).ToList();

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

        [Authorize]
        public ActionResult TrackSellerapi(int sellerid)//移除追蹤商家
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();

            var trackSellerId=db.TrackSellers.FirstOrDefault(x=>x.UserId==userid&&x.SellerId== sellerid);
            if (trackSellerId == null)
            {
                var data = new TrackSeller()
                {
                    UserId=userid,
                    SellerId=sellerid,
                };
                db.TrackSellers.Add(data);
                db.SaveChanges();
                return Content("TrackSeller");
            }
            else
            {
                var trackSellerIdToDelete = trackSellerId.TrackSellerId;
                var trackSellerToDelete = db.TrackSellers.FirstOrDefault(x => x.TrackSellerId == trackSellerIdToDelete && x.SellerId == sellerid);
                if(trackSellerToDelete != null)
                {
                    db.TrackSellers.Remove(trackSellerToDelete);
                    db.SaveChanges();
                }
                return Content("noTrackSeller");
            }
        }
    }
}