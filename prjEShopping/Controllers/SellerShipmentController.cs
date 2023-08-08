    using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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
            int sellerid = (int)Session["SellerId"];
            List<SellerShipmentVM> datashow;
            AppDbContext db = new AppDbContext();

            var data = db.Shipments.Where(x => x.SellerId == sellerid)
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

        public ActionResult ShipmentDetail(string ShipNum, string Shiptatus)
        {
            int sellerid = (int)Session["SellerId"];
            //寄件明細
            var db = new AppDbContext();
            var data = db.ShipmentDetails.Where(x => x.ShipmentNumber == ShipNum).SingleOrDefault();
            ViewBag.Smethod = (db.ShippingMethods.Where(x => x.ShippingMethodId == data.ShippingMethodId).FirstOrDefault()).ShippingMethodName;
            ViewBag.Pmethod = (db.PaymentMethods.Where(x => x.PaymentMethodId == data.PaymentMethodId).FirstOrDefault()).PaymentMethodName;
            ViewBag.Receiver = data.Receiver;
            ViewBag.RAddress = data.ReceiverAddress;
            ViewBag.Fright = (int)db.ShippingMethods.Where(x => x.ShippingMethodId == data.ShippingMethodId).FirstOrDefault().Freight;
            ViewBag.ShipmentNumber = ShipNum;
            ViewBag.ShipmentStatus = Shiptatus;
            ViewBag.ShipmentDate = db.Shipments.FirstOrDefault(x => x.ShipmentNumber == ShipNum).ShipmentDate;
            ViewBag.ShipDate = db.Shipments.FirstOrDefault(x => x.ShipmentNumber == ShipNum).ShipDate;
            ViewBag.ArrivalTimeDate = db.Shipments.FirstOrDefault(x => x.ShipmentNumber == ShipNum).ArrivalTimeDate;
            ViewBag.PickDate = db.Shipments.FirstOrDefault(x => x.ShipmentNumber == ShipNum).PickDate;
            ViewBag.CompletionDate = db.Shipments.FirstOrDefault(x => x.ShipmentNumber == ShipNum).CompletionDate;
            //購買人
            var userorderid = db.Shipments.Where(x => x.ShipmentNumber == ShipNum).SingleOrDefault();

            //拆開版
            //var userid = db.Orders.Where(y => y.OrderId == userorderid.OrderId).FirstOrDefault().UserId;

            //濃縮版
            var username = db.Users.Where(x => x.UserId == (db.Orders.Where(y => y.OrderId == userorderid.OrderId).FirstOrDefault().UserId)).SingleOrDefault().UserName;
            ViewBag.username = username;

            //購買內容
            var products = db.OrderDetails.Where(x => x.OrderId == userorderid.OrderId).Join(db.Products.Where(y => y.SellerId == sellerid), x => x.ProductId, y => y.ProductId, (x, y) => new
            {
                ProductId = y.ProductId,
                ProductName = y.ProductName,
                ProductImage = y.ProductImagePathOne,
                ProductPrice = x.CurrentPrice,
                Qty = x.Quantity,
            })
            .ToList();

            List<SellerShipmentDetailsVM> details = products.Select(x => new SellerShipmentDetailsVM
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Price = Convert.ToInt32(x.ProductPrice),
                Quantity = (int)x.Qty,
                ProductImagePathOne = x.ProductImage,
            }).ToList();

            return View(details);
        }

        //      [HttpPost]
        //public ActionResult ShipmentDetail (SellerShipmentDetailsVM vm)
        //      {
        //	var db = new AppDbContext();


        //	return RedirectToAction("ShipmentDetail");
        //}

        public ActionResult Cancle(string ShipNum)
        {
            int sellerid = (int)Session["SellerId"];
            var db = new AppDbContext();

            var userorderid = db.Shipments.Where(x => x.ShipmentNumber == ShipNum).SingleOrDefault();

            var products = db.OrderDetails.Where(x => x.OrderId == userorderid.OrderId).Join(db.Products.Where(y => y.SellerId == sellerid), x => x.ProductId, y => y.ProductId, (x, y) => new
            {
                ProductId = y.ProductId,
                ChageQty = x.Quantity,
            })
            .ToList().Join(db.ProductStocks, x => x.ProductId, y => y.ProductId, (x, y) => new
            {
                ProductStockId = y.ProductStockId,
                ProductId = x.ProductId,
                ChageQty = x.ChageQty,
                OrderQuantity = y.OrderQuantity,
                StockQuantity = y.StockQuantity
            }).ToList();

            foreach (var items in products)
            {
                var change = db.ProductStocks.Find(items.ProductStockId);
                change.OrderQuantity = change.OrderQuantity - items.ChageQty;
                change.StockQuantity = change.StockQuantity + items.ChageQty;
            }

            var Shiptatusid = db.Shipments.Where(x => x.ShipmentNumber == ShipNum).SingleOrDefault();

            Shiptatusid.ShipmentStatusId = 6;

            db.SaveChanges();

            var Shiptatus = db.ShipmentStatusDetails.Where(x => x.ShipmentStatusId == 6).FirstOrDefault().ShipmentStatus;

            var parameters = new RouteValueDictionary
            {
                { "ShipNum", ShipNum },
                { "Shiptatus", Shiptatus },

            };
            return RedirectToAction("ShipmentDetail", parameters);
        }

        public ActionResult ToShip(string ShipNum)
        {
            int sellerid = (int)Session["SellerId"];
            var db = new AppDbContext();

            var userorderid = db.Shipments.Where(x => x.ShipmentNumber == ShipNum).SingleOrDefault();

            var products = db.OrderDetails.Where(x => x.OrderId == userorderid.OrderId).Join(db.Products.Where(y => y.SellerId == sellerid), x => x.ProductId, y => y.ProductId, (x, y) => new
            {
                ProductId = y.ProductId,
                ChageQty = x.Quantity,
            })
            .ToList().Join(db.ProductStocks, x => x.ProductId, y => y.ProductId, (x, y) => new
            {
                ProductStockId = y.ProductStockId,
                ProductId = x.ProductId,
                ChageQty = x.ChageQty,
                OrderQuantity = y.OrderQuantity,
                StockQuantity = y.StockQuantity
            }).ToList();

            foreach (var items in products)
            {
                var change = db.ProductStocks.Find(items.ProductStockId);
                change.OrderQuantity = change.OrderQuantity - items.ChageQty;
                change.QuantitySold = change.QuantitySold + items.ChageQty;
                change.StockQuantity = change.StockQuantity - items.ChageQty;
            }

            var Shiptatusid = db.Shipments.Where(x => x.ShipmentNumber == ShipNum).SingleOrDefault();

            Shiptatusid.ShipmentStatusId = 2;
            Shiptatusid.ShipDate = DateTime.Now;
            db.SaveChanges();

            var Shiptatus = db.ShipmentStatusDetails.Where(x => x.ShipmentStatusId == 2).FirstOrDefault().ShipmentStatus;

            var parameters = new RouteValueDictionary
            {
                { "ShipNum", ShipNum },
                { "Shiptatus", Shiptatus },

            };
            return RedirectToAction("ShipmentDetail", parameters);

        }

        public ActionResult setToRecieve(string ShipNum) 
        {
            var db = new AppDbContext();
            var Shiptatusid = db.Shipments.FirstOrDefault(x => x.ShipmentNumber == ShipNum);

            Shiptatusid.ShipmentStatusId = 3;
            Shiptatusid.ArrivalTimeDate = DateTime.Now;
            db.SaveChanges();

            var Shiptatus = db.ShipmentStatusDetails.FirstOrDefault(x => x.ShipmentStatusId == 3).ShipmentStatus;

            var parameters = new RouteValueDictionary
            {
                { "ShipNum", ShipNum },
                { "Shiptatus", Shiptatus },

            };
            return RedirectToAction("ShipmentDetail", parameters);
        }

        public ActionResult setToComplete(string ShipNum)
        {
            var db = new AppDbContext();
            var Shiptatusid = db.Shipments.FirstOrDefault(x => x.ShipmentNumber == ShipNum);

            Shiptatusid.ShipmentStatusId = 5;
            Shiptatusid.CompletionDate = DateTime.Now;
            db.SaveChanges();

            var Shiptatus = db.ShipmentStatusDetails.FirstOrDefault(x => x.ShipmentStatusId == 5).ShipmentStatus;

            var parameters = new RouteValueDictionary
            {
                { "ShipNum", ShipNum },
                { "Shiptatus", Shiptatus },

            };
            return RedirectToAction("ShipmentDetail", parameters);
        }

        public ActionResult getTop10Shipments() 
        {
            var db = new AppDbContext();
            var top10Shipments = db.Shipments.Where(x => x.SellerId == 1 && x.ShipmentStatusId == 1)
                            .Join(db.ShipmentStatusDetails,x => x.ShipmentStatusId,y => y.ShipmentStatusId,(x,y) => new 
                            { 
                                shipmentdate = x.ShipmentDate,
                                shipmentnumber = x.ShipmentNumber,
                                shipmentstatus = y.ShipmentStatus,
                            }).OrderByDescending(x => x.shipmentdate).Take(10).ToList().Select(x => new 
                            {
                                shipmentdate = x.shipmentdate.ToString(),
                                shipmentnumber = x.shipmentnumber,
                                shipmentstatus = x.shipmentstatus,
                            });
            return Json(top10Shipments,JsonRequestBehavior.AllowGet);
        }
    }
}