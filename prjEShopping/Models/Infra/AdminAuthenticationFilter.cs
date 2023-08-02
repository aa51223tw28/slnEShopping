using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace prjEShopping.Models.Infra
{
    public class AdminAuthenticationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpCookie authCookie = filterContext.HttpContext.Request.Cookies["AdminLogin"];
            if (authCookie != null && authCookie.Values["status"] == "AdminLogin" && authCookie.Values["AccessRightId"] == "2")
            {
                // 如果符合條件，可以直接重定向
               
                //filterContext.Result = new RedirectToRouteResult(
                //    new RouteValueDictionary
                //    {
                //    { "controller", "Admins" },
                //    { "action", "Index" }
                //    });            
                    filterContext.Controller.ViewBag.AdminName = authCookie["AdminName"];
            }
            else
            {
                // 如果不符合條件，你可以進一步處理，例如重定向到登錄頁面
                filterContext.Result = new RedirectToRouteResult(
            new RouteValueDictionary
            {
                    { "controller", "Admins" },
                    { "action", "Login" }
            });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}