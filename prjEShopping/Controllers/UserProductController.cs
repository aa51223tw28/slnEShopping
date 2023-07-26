using prjEShopping.Models.DTOs;
using prjEShopping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjEShopping.Controllers
{
    public class UserProductController : Controller
    {
        // GET: UserProduct
        public ActionResult UserProdutList()
        {
            var db = new AppDbContext();
            List<UserProductIndexDto> datas = new List<UserProductIndexDto>();

            var products = db.Products.ToList();

            foreach (var item in products)
            {
                var orderQuantity = db.ProductStocks.Where(x => x.ProductId == item.ProductId).Select(x => x.OrderQuantity).FirstOrDefault() ?? 0;

                var stockQuantity = db.ProductStocks.Where(x => x.ProductId == item.ProductId).Select(x => x.StockQuantity).FirstOrDefault() ?? 0;

                if (stockQuantity - orderQuantity > 0)
                {
                    var data = new UserProductIndexDto()
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Price = (decimal)item.Price,
                        ProductImagePathOne = item.ProductImagePathOne,
                        ProductStock = stockQuantity - orderQuantity
                    };

                    datas.Add(data);
                }
            }
            return View(datas);
        }

        public ActionResult UserSingleProduct(int productId)
        {            
            var db=new AppDbContext();
            var product = db.Products.FirstOrDefault(x => x.ProductId == productId);

            //剩餘多少數量計算式為ProductStocks table=StockQuantity-OrderQuantity
            var orderQuantity = db.ProductStocks.Where(x => x.ProductId == productId).Select(x=>x.OrderQuantity).FirstOrDefault() ?? 0;
            var stockQuantity= db.ProductStocks.Where(x => x.ProductId == productId).Select(x => x.StockQuantity).FirstOrDefault() ?? 0;
            
            var datas = new UserProductIndexDto()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Price = (decimal)product.Price,
                ProductImagePathOne = product.ProductImagePathOne,
                ProductImagePathTwo = product.ProductImagePathTwo,
                ProductImagePathThree = product.ProductImagePathThree,
                ProductStock= stockQuantity- orderQuantity
            };

            return View(datas);
        }
    }
}