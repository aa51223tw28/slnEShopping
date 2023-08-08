using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using GoogleAuthentication.Services;
using Newtonsoft.Json;
using prjEShopping.Models.ViewModels;
using prjEShopping.Models.EFModels;

namespace prjEShopping.Controllers
{
    public class GoogleController : Controller
    {
        // GET: Google
        AppDbContext db = new AppDbContext();
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
            if (token != null)
            {
                var userProfile = await GoogleAuth.GetProfileResponseAsync(token.AccessToken.ToString());
                var googleUser = JsonConvert.DeserializeObject<GoogleProfileVM>(userProfile);

                // Check if the user is already registered in your database
                var existingUser = db.Sellers.FirstOrDefault(u => u.SellerAccount == googleUser.Email);

                if (existingUser != null)
                {
                    return RedirectToAction("Index", "SellerMain");
                }
                else
                {
                    return RedirectToAction("Login", "SellerLogin");
                }
            }
            else
            {
                return RedirectToAction("Error", "SellerLogin");
            }
        }

    }
}