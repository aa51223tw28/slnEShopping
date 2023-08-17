using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class UserChatController : Controller
    {
        // GET: UserChat
        public ActionResult UserChat()
        {
            var db = new AppDbContext();
            var customerAccount = User.Identity.Name;
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var username = db.Users.FirstOrDefault(x => x.UserAccount == customerAccount).UserName;
            //userid待加*2地方

            var chatMembersList = db.ChatroomMembers.Where(x => x.UserId == userid).Join(db.Sellers,x => x.SellerId,y => y.SellerId, (x, y) => new UserChatVM{
                SellerId = y.SellerId,
                StoreName = y.StoreName,
                ChatroomId = (x.ChatroomId.ToString()),
                UserName = username,
            }).ToList();
            return View(chatMembersList);
        }

        public ActionResult FindChatroom(int sellerid) 
        {
            var db = new AppDbContext();
            var customerAccount = User.Identity.Name;
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var findChatroom = db.ChatroomMembers.FirstOrDefault(x => x.UserId == userid && x.SellerId == sellerid);
            if (findChatroom == null) 
            {
                var addToChatroom = new ChatroomMember
                {
                    SellerId = sellerid,
                    UserId = userid,
                };
                db.ChatroomMembers.Add(addToChatroom);
                db.SaveChanges();
            }
            return RedirectToAction("UserChat");
        }

    }
}