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
                                    .Join(db.ShipmentStatusDetails, x => x.ShipmentStatusId, y => y.ShipmentStatusId, (x, y) => new
                                    {
                                        ShipmentStatus = y.ShipmentStatus,
                                        ShipmentDate = x.ShipmentDate,
                                        ShipmentNumber = x.ShipmentNumber,
                                    }).ToList();
            datashow = data.Select(x => new SellerShipmentVM
            {
                ShipmentStatus = x.ShipmentStatus,
                ShipmentDate = (DateTime)x.ShipmentDate,
                ShipmentNumber = x.ShipmentNumber,
            }).ToList();

            return View(datashow);
        }

        public ActionResult ShipmentDetail(string ShipNum)
        {
            var db = new AppDbContext();
            var data = db.ShipmentDetails.Where(x => x.ShipmentNumber == ShipNum).SingleOrDefault();
            ViewBag.Smethod = db.ShippingMethods.Where(x => x.ShippingMethodId == data.ShippingMethodId).Select(x => x.ShippingMethodName);
            ViewBag.Pmethod = db.PaymentMethods.Where(x => x.PaymentMethodId == data.PaymentMethodId).Select(x => x.PaymentMethodName);
            ViewBag.Recriver = data.Receiver;
            ViewBag.RAddress = data.ReceiverAddress;

			return View();
        }
    }
}