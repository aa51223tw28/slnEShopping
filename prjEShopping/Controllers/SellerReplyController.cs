using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class SellerReplyController : Controller
    {
        // GET: SellerReply
        private readonly AppDbContext db = new AppDbContext();

        [HttpPost]
        public ActionResult AddReply(int commentId, string replyText)
        {
            // Add reply logic here
            // ...
            return RedirectToAction("Index", "BuyerFeedback"/*, new { sellerId = /*sellerIdFromComment*/ );
        }
    }
}