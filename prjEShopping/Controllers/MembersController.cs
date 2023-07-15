using prjEShopping.Models.EFModels;
using prjEShopping.Models.Infra;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace prjEShopping.Controllers
{
    public class MembersController : Controller
    {
        // GET: Members
        public ActionResult Login()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult Login(LoginVM vm)
        //{
        //    if(ModelState.IsValid==false) return View(vm);//以便用戶能夠看到錯誤訊息和原始輸入值，並進行必要的修正

        //    //驗證帳密的正確性
        //    Result result


        //}

        //private Result VailLogin(LoginVM vm)
        //{
        //    var db = new AppDbContext();
        //    var member=db.Users.FirstOrDefault(x=>x.UserAccount==vm.UserAccount);

        //    if (member == null) return Result.Fail("帳密有誤");

        //    //會員資格要是AccessRightId==1(使用中)
        //    var right = member.AccessRightId.ToString();

        //    if (right != "1")
        //    {
        //        return Result.Fail("會員資格尚未確認");
        //    }

        //    var 
        //}
    }
}