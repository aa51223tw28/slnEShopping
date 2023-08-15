using prjEShopping.Models.DTOs;
using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class MainController : Controller
    {
        // GET: UserMain
        public ActionResult Index()
        {
            //--Admin coolie 登入紀錄---
            HttpCookie authCookie = Request.Cookies["AdminLogin"];
            if (authCookie == null || authCookie.Values["status"] != "AdminLogin" || authCookie.Values["AccessRightId"] != "1")
            {
                return View();
            }
            string decodedName = HttpUtility.UrlDecode(authCookie.Values["userName"]);
            ViewBag.AdminName = decodedName;
            //---Admin coolie 登入紀錄---

           

            return View();
        }

        public ActionResult SearchSubcategoryIdapi(string subcategoryName)
        {
            var db = new AppDbContext();
            var subcategoryId = db.ProductSubCategories.Where(x => x.SubcategoryName == subcategoryName).Select(x=>x.SubcategoryId).FirstOrDefault();
            return Json(subcategoryId, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTop5ProductMain()
        {
            var db = new AppDbContext();
            var products = db.Products.Where(x => x.ProductStatusId == 2)//ProductStatusId=2為販售中
                                        .Join(db.ProductStocks, x => x.ProductId, y => y.ProductId, (x, y) => new
                                        {
                                            x.ProductId,
                                            x.ProductImagePathOne,
                                            x.ProductName,
                                            x.Price,//價錢沒考慮折扣後
                                            y.QuantitySold,
                                            y.OrderQuantity,
                                            y.StockQuantity,
                                            x.SubcategoryId,
                                        })
                                        .Where(x=>x.StockQuantity>= x.OrderQuantity)//庫存>訂單量
                                        .OrderByDescending(x => x.QuantitySold).Take(5);
            return Json(products, JsonRequestBehavior.AllowGet);
        }


        public ActionResult MaleSale()//限時特賣
        {
            var db = new AppDbContext();
            
            var model = db.Products.Join(db.ADProducts,x => x.ProductId,y => y.ProductId, (x, y) => 
                                        new UserProductIndexDto
                                        {
                                            ProductId = x.ProductId,
                                            ProductName = x.ProductName,
                                            Price = (decimal)x.Price,
                                            Discount = (int)y.Discount,                                            
                                            ProductImagePathOne = x.ProductImagePathOne
                                        }).ToList();

            foreach (var item in model)
            {               
                var discountedPrice = (item.Price*(item.Discount))/100;
                item.DiscountPrice = discountedPrice;
            }

            return View(model);
        }
    }
}