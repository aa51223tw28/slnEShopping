using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;

namespace prjEShopping.Controllers
{
    public class ADPointsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: ADPoints
        public ActionResult List()
        {
            var model= db.ADPoints.ADPoint2VM().ToList();
            var Sellers=db.Sellers.ToList();
            ViewBag.Sellers = Sellers;
            return View(model);
        }

        [HttpPost]
        public ActionResult List(string search)
        {

            int sellerId;
            if (int.TryParse(search, out sellerId))
            {
                var model = db.ADPoints.Where(x => x.SellerId == sellerId).ADPoint2VM().ToList();
                return View(model);
            }

            else
            {
                return RedirectToAction("List");
            }
        }

            // GET: ADPoints/Details/5
            public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ADPoint aDPoint = db.ADPoints.Find(id);
            if (aDPoint == null)
            {
                return HttpNotFound();
            }
            return View(aDPoint);
        }

        // GET: ADPoints/BuyPoints
        public ActionResult BuyPoints(int? sellerId=1)
        {
            sellerId = 1;
            var point=new ADPoint();
            var model =ADPointChange.BuyPoint(point,1);
            ViewBag.List = db.ADPoints.Where(s=>s.SellerId==sellerId).OrderByDescending(d=>d.PurchaseTime).ToList();
            ViewBag.SellerPoint=db.Sellers.Where(s => s.SellerId == sellerId).Select(p=>p.ADPoints).FirstOrDefault();
            return View(model);
        }

        // POST: ADPoints/BuyPoints
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BuyPoints([Bind(Include = "ADPointId,SellerId,GUINumber,PurchaseTime,ADPoints,PaymentStatus")] ADPointVM vm)
        {

            var sellerToUpdate = db.Sellers.FirstOrDefault(x => x.SellerId == vm.SellerId);
            if (sellerToUpdate != null)
            {
                sellerToUpdate.ADPoints = sellerToUpdate.ADPoints+vm.ADPoints;
                db.Entry(sellerToUpdate).State = EntityState.Modified; 
                db.ADPoints.Add(ADPointChange.VM2ADPoint(vm));
                db.SaveChanges();
                return RedirectToAction("BuyPoints");
            }

            return View(vm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
