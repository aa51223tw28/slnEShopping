using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class SellerProductController : Controller
    {
        // GET: SellerProduct
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult ProductList()
		{
			return View();
		}
	}
}