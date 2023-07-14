using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
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
            List<SellerShipmentVM> datashow;
            AppDbContext db = new AppDbContext();

            var data = db.Shipments.Where(x => x.SellerId == 1)
                                    .Join(db.ShipmentStatusDetails,x =>x.ShipmentStatusId, y => y.ShipmentStatusId,(x,y) => new 
                                    {
                                        ShipmentStatus = y.ShipmentStatus,
                                        ShipmentDate = x.ShipmentDate,
                                        ShipmentNumber = x.ShipmentNumber,
                                    }).ToList();
            datashow = data.Select(x => new SellerShipmentVM {
                ShipmentStatus = x.ShipmentStatus,
                ShipmentDate = (DateTime)x.ShipmentDate,
                ShipmentNumber = x.ShipmentNumber,
            }).ToList();

            return View(datashow);
		}
	}
}