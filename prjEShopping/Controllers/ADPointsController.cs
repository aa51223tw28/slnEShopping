using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
        public ActionResult Index()
        {
            return View(db.ADPoints.ToList());
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

        // GET: ADPoints/Create
        public ActionResult Create(int? sellerId)
        {
            sellerId = 6;
            var model = db.ADPoints.ADPoint2VM().FirstOrDefault();
            ViewBag.List = db.ADPoints.Where(s=>s.SellerId==sellerId).OrderByDescending(d=>d.PurchaseTime).ToList();
            ViewBag.SellerPoint=db.Sellers.Where(s => s.SellerId == sellerId).Select(p=>p.ADPoints).FirstOrDefault();
            return View(model);
        }

        // POST: ADPoints/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ADPointId,SellerId,GUINumber,PurchaseTime,ADPoints,PaymentStatus")] ADPointVM vm)
        {
            var sellerId = 6;
            if (ModelState.IsValid)
            {            
                var Seller=db.Sellers.FirstOrDefault(x=>x.SellerId == sellerId);
                Seller.ADPoints = Seller.ADPoints+ADPointChange.VM2ADPoint(vm).ADPoints;             
                db.ADPoints.Add(ADPointChange.VM2ADPoint(vm));
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            return View(vm);
        }

        // GET: ADPoints/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: ADPoints/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ADPointId,SellerId,GUINumber,PurchaseTime,ADPoints,PaymentStatus")] ADPoint aDPoint)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aDPoint).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aDPoint);
        }

        // GET: ADPoints/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: ADPoints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ADPoint aDPoint = db.ADPoints.Find(id);
            db.ADPoints.Remove(aDPoint);
            db.SaveChanges();
            return RedirectToAction("Index");
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
