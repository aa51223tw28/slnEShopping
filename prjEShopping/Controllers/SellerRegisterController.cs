﻿using prjEShopping.Models;
using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class SellerRegisterController : Controller
    {
        public ActionResult List()
        {
            AppDbContext db = new AppDbContext();
            var datas = from t in db.Sellers
                        select t;
            return View(datas);
        }
        
        // GET: SellerRegister
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Seller s)
        {
            if (String.IsNullOrEmpty(s.GUINumber))
            {
                TempData["Fail"] = "統編為必填欄位！";
                return View();
            }
            if (String.IsNullOrEmpty(s.SellerName))
            {
                TempData["Fail"] = "負責人姓名為必填欄位！";
                return View();
            }
            if (String.IsNullOrEmpty(s.StoreName))
            {
                TempData["Fail"] = "商家姓名為必填欄位！";
                return View();
            }
            if (String.IsNullOrEmpty(s.SellerAccount))
            {
                TempData["Fail"] = "電子郵件為必填欄位！";
                return View();
            }
            if (String.IsNullOrEmpty(s.SellerPassword))
            {
                TempData["Fail"] = "使用者密碼為必填欄位！";
                return View();
            }
            if (String.IsNullOrEmpty(s.Address))
            {
                TempData["Fail"] = "住址為必填欄位！";
                return View();
            }
            if (String.IsNullOrEmpty(s.Phone))
            {
                TempData["Fail"] = "手機號碼為必填欄位！";
                return View();
            }
            if (String.IsNullOrEmpty(s.BankAccount))
            {
                TempData["Fail"] = "銀行帳戶為必填欄位！";
                return View();
            }
            if (String.IsNullOrEmpty(s.StoreIntro))
            {
                TempData["Fail"] = "商城介紹為必填欄位！";
                return View();
            }
            var db = new AppDbContext();
            db.Sellers.Add(s);
            db.SaveChanges();
            TempData["Success"] = "您已註冊成功！";
            
            return View("~/Views/SellerMain/Index.cshtml");
            //return Response.Write(msg);

        }


        //public ActionResult Save()
        //{
        //    SellerRegisterVM s = new SellerRegisterVM();
        //    s.GUINumber = Request.Form["txtGUINumber"];
        //    s.SellerName = Request.Form["txtSellerName"];
        //    s.StoreName = Request.Form["txtStoreName"];
        //    s.SellerAccount = Request.Form["txtSellerAccount"];
        //    s.SellerPassword = Request.Form["txtSellerPassword"];
        //    s.Address = Request.Form["txtAddress"];
        //    s.Phone = Request.Form["txtPhone"];
        //    s.BankAccount = Request.Form["txtBankAccount"];
        //    s.StoreIntro = Request.Form["txtStoreIntro"];
        //     (new CFactory()).create(s);
        //    return RedirectToAction("List");
        //}
    }
}