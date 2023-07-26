using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class SellerProductCreateController : Controller
    {
        // GET: SellerProduct
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProductCreate()
        {
            return View();
        }
    }
}

