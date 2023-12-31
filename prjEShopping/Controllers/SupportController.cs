﻿using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class SupportController : Controller
    {
        AppDbContext db = new AppDbContext();
        // GET: Support
        public ActionResult Index()
        {
            var userid = 2;
            var model = db.Supports.Where(x => x.UserId == userid && x.SupportStatus != "3").OrderBy(x => x.SupportStatus).OrderByDescending(y => y.ReceivedTime).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            var s = db.Supports.FirstOrDefault(x => x.SupportId == id);
            if (s != null)
            {
                s.SupportStatus = "3";
                db.Entry(s).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
        

        public ActionResult CSEmail()
        {
            var userId = 1;
            var productId = 1;
            var model = db.Supports.FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        public ActionResult CSEmail(Support s)
        {
            var sup = new Support();
            sup = s;
            db.Supports.Add(sup);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult CSEmailDetails(int? id)
        {
            //todo 還要設定UserId權限 不同的點不開回前一頁等
            var s = db.Supports.FirstOrDefault(x => x.SupportId == id);
            var r = db.SupportReplaies.FirstOrDefault(x => x.SupportId == id);
            var model = new CombinedSupportsVM
            {
                Support = s,
                SupportReplay = r,
            };
            return View(model);
        }

        public ActionResult IndexBtn()
        {
            var userid = 1;
            var sup = new Support();
            ViewBag.Userid = userid;
            ViewBag.Support = sup;
            return View();
        }

        //Customer Service System CSS
        public ActionResult CSSIndex()
        {
            return View();
        }

        public ActionResult CSSNewMailList()
        {
            var model = db.Supports.Where(x => x.SupportStatus == "1").ToList();
            return View(model);
        }

        public ActionResult CSSNewMailReplay(int? id)
        {
            var model = db.Supports.FirstOrDefault(x => x.SupportId == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult CSSNewMailReplay(SupportReplay r)
        {
            var replay = new SupportReplay();
            replay = r;
            replay.AdminId = 1;
            var s = db.Supports.FirstOrDefault(x => x.SupportId == r.SupportId);
            if (s != null)
            {
                s.SupportStatus = "2";
                db.Entry(s).State = EntityState.Modified;
                db.SupportReplaies.Add(replay);
                db.SaveChanges();
                return RedirectToAction("CSSIndex");
            }
            return View(r);
        }

        public ActionResult CSSOldMailList()
        {
            var model = db.Supports.Where(x => x.SupportStatus == "2").ToList();
            return View(model);
        }

        public ActionResult CSSOldMailDetails(int? id)
        {
            var s = db.Supports.FirstOrDefault(x => x.SupportId == id);
            var r=db.SupportReplaies.FirstOrDefault(x => x.SupportId == id);
            var model = new CombinedSupportsVM
            {
                Support = s,
                SupportReplay = r,
            };
            return View(model);
        }

    }
}