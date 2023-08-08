using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using GoogleAuthentication.Services;

namespace prjEShopping.Controllers
{
    public class GoogleController : Controller
    {
        // GET: Google
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> RedirectGoogleLogin(string code)
        {
            var clientId = "428102972567-vrsafsab1n7en9tuu7k7lajf77dcmcgo.apps.googleusercontent.com";
            var url = "https://localhost:8080/User/RedirectGoogleLogin";
            var clientsecret = "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";
            var token=await GoogleAuth.GetAuthAccessToken(code,clientId,clientsecret,url);
            var userProfile=await GoogleAuth.GetProfileResponseAsync(token.AccessToken.ToString());
            return View();
        }

    }
}