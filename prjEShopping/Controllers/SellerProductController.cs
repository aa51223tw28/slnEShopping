using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
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
        public ActionResult SellerProductList()
        {
            var db = new AppDbContext();
            var products = db.Products.Where(x => x.SellerId == 1).Join(db.ProductStocks,x => x.ProductId,y => y.ProductId,(x,y) => new
            {
                ProductID = x.ProductId,
                ProductName = x.ProductName,
                Price = x.Price,
                ProductImagePathOne = x.ProductImagePathOne,
                StockQuantity = y.StockQuantity,
                OrderQuantity = y.OrderQuantity,
                ProductStatusId = x.ProductStatusId
            }).Select(x => new SellerProductListVM
            {
                ProductID = x.ProductID,
                ProductName = x.ProductName,
                Price = (int)x.Price,
                ProductImagePathOne = x.ProductImagePathOne,
                StockQuantity = x.StockQuantity,
                OrderQuantity = x.OrderQuantity,
                ProductStatusName = (db.ProductStatusDetails.Where(y => y.ProductStatusId == x.ProductStatusId).FirstOrDefault().ProductStatusName).ToString()
            }).ToList();

            return View(products);
        }

        public ActionResult ChangeStatus(int? id) 
        {
            var db = new AppDbContext();
            var product= db.Products.Find(id);
            product.ProductStatusId = 1;
            db.SaveChanges();

            return RedirectToAction("SellerProductList");
        }
    }
}