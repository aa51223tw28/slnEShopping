﻿using prjEShopping.Models.EFModels;
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
            //寄件明細
            var db = new AppDbContext();
            var data = db.ShipmentDetails.Where(x => x.ShipmentNumber == ShipNum).SingleOrDefault();
            ViewBag.Smethod = (db.ShippingMethods.Where(x => x.ShippingMethodId == data.ShippingMethodId).FirstOrDefault()).ShippingMethodName;
            ViewBag.Pmethod = (db.PaymentMethods.Where(x => x.PaymentMethodId == data.PaymentMethodId).FirstOrDefault()).PaymentMethodName;
            ViewBag.Recriver = data.Receiver;
            ViewBag.RAddress = data.ReceiverAddress;
            ViewBag.Fright = (db.ShippingMethods.Where(x => x.ShippingMethodId == data.ShippingMethodId).FirstOrDefault()).Freight;

			//購買人
			var userorderid = db.Shipments.Where(x => x.ShipmentNumber == ShipNum).SingleOrDefault();

            //拆開版
            //var userid = db.Orders.Where(y => y.OrderId == userorderid.OrderId).FirstOrDefault().UserId;

            //濃縮版
			var username = db.Users.Where(x => x.UserId == (db.Orders.Where(y => y.OrderId == userorderid.OrderId).FirstOrDefault().UserId)).SingleOrDefault().UserName;
            ViewBag.username = username;
			return View();
        }
    }
}