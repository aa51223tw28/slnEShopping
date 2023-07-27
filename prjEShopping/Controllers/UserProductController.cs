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
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var cartid = db.ShoppingCarts.Where(x => x.UserId == userid).OrderByDescending(x => x.CartId).Select(x => x.CartId).FirstOrDefault();

            List<UserProductIndexDto> datas = new List<UserProductIndexDto>();

            var products = db.Products.ToList();

            foreach (var item in products)
            {
                var orderQuantity = db.ProductStocks.Where(x => x.ProductId == item.ProductId).Select(x => x.OrderQuantity).FirstOrDefault() ?? 0;

                var stockQuantity = db.ProductStocks.Where(x => x.ProductId == item.ProductId).Select(x => x.StockQuantity).FirstOrDefault() ?? 0;

                //商品狀態 1審核中/2販售中/3下架中
                var productstatus = products.Where(x => x.ProductId == item.ProductId).Select(x => x.ProductStatusId).FirstOrDefault();
                

                if (stockQuantity - orderQuantity > 0&& productstatus==2)
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
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var cartid = db.ShoppingCarts.Where(x => x.UserId == userid).OrderByDescending(x => x.CartId).Select(x => x.CartId).FirstOrDefault();

            var product = db.Products.FirstOrDefault(x => x.ProductId == productId);

            //剩餘多少數量計算式為ProductStocks table=StockQuantity-OrderQuantity
            var orderQuantity = db.ProductStocks.Where(x => x.ProductId == productId).Select(x=>x.OrderQuantity).FirstOrDefault() ?? 0;
            var stockQuantity= db.ProductStocks.Where(x => x.ProductId == productId).Select(x => x.StockQuantity).FirstOrDefault() ?? 0;

            //售出數量ProductStocks中的QuantitySold
            var quantitySold = db.ProductStocks.Where(x => x.ProductId == productId).Select(x => x.QuantitySold).FirstOrDefault();

            //品牌
            var brandId = db.Products.Where(x => x.ProductId == productId).Select(x => x.BrandId).FirstOrDefault();
            var brandName = db.Brands.Where(x => x.BrandId == brandId).Select(x => x.BrandName).FirstOrDefault();

            var datas = new UserProductIndexDto()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Price = (decimal)product.Price,
                ProductImagePathOne = product.ProductImagePathOne,
                ProductImagePathTwo = product.ProductImagePathTwo,
                ProductImagePathThree = product.ProductImagePathThree,
                ProductStock= stockQuantity- orderQuantity,
                QuantitySold= (int)quantitySold,

                //一堆規格
                BrandName= brandName,


            };
            //現在購物車該使用者該購物車id該商品的數量
            var qua = db.ShoppingCartDetails.Where(x => x.CartId == cartid && x.ProductId == productId).Select(x => x.Quantity).FirstOrDefault();
            if(qua != null)
            {
                ViewBag.TotalQuantity = qua;
            }
            else
            {
                ViewBag.TotalQuantity = 0;
            }            
            return View(datas);
        }
    }
}