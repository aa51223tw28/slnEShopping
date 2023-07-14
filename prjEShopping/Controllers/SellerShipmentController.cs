using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class SellerShipmentController : Controller
    {
        // GET: SellerShipment
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult ShipmentList()
		{
            
			return View();
		}
	}
}