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
                        ProductStock = stockQuantity - orderQuantity,
                        QuantitySold= (int)db.ProductStocks.FirstOrDefault(x => x.ProductId == item.ProductId).QuantitySold,
                        SubcategoryId= (int)products.FirstOrDefault(x=>x.ProductId== item.ProductId).SubcategoryId,
                        BrandId=(int)item.BrandId,
                        //BrandName=db.Brands.FirstOrDefault(x=>x.BrandId== item.BrandId).BrandName,
                    };

                    datas.Add(data);
                }
            }

            ViewBag.Brand = db.Brands.ToList();

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

            //一堆規格
            //品牌
            var brandId = db.Products.Where(x => x.ProductId == productId).Select(x => x.BrandId).FirstOrDefault();
            var brandName = db.Brands.Where(x => x.BrandId == brandId).Select(x => x.BrandName).FirstOrDefault();                     

            //選項內容
            var optionIdOne = db.Products.Where(x => x.ProductId == productId).Select(x => x.OptionIdOne).FirstOrDefault();
            var optionIdTwo = db.Products.Where(x => x.ProductId == productId).Select(x => x.OptionIdTwo).FirstOrDefault();
            var optionIdThree = db.Products.Where(x => x.ProductId == productId).Select(x => x.OptionIdThree).FirstOrDefault();
            var optionIdFour = db.Products.Where(x => x.ProductId == productId).Select(x => x.OptionIdFour).FirstOrDefault();
            var optionIdFive = db.Products.Where(x => x.ProductId == productId).Select(x => x.OptionIdFive).FirstOrDefault();

            int parsedOptionIdOne = int.Parse(optionIdOne);
            int parsedOptionIdTwo = int.Parse(optionIdTwo);
            int parsedOptionIdThree = int.Parse(optionIdThree);
            int parsedOptionIdFour = int.Parse(optionIdFour);
            int parsedOptionIdFive = int.Parse(optionIdFive);


            var optionNameOne = db.ProductOptions.Where(x => x.OptionId == parsedOptionIdOne).Select(x => x.OptionName).FirstOrDefault();
            var optionNameTwo = db.ProductOptions.Where(x => x.OptionId == parsedOptionIdTwo) .Select(x => x.OptionName).FirstOrDefault();
            var optionNameThree = db.ProductOptions.Where(x => x.OptionId == parsedOptionIdThree).Select(x => x.OptionName).FirstOrDefault();
            var optionNameFour = db.ProductOptions.Where(x => x.OptionId == parsedOptionIdFour).Select(x => x.OptionName).FirstOrDefault();
            var optionNameFive = db.ProductOptions.Where(x => x.OptionId == parsedOptionIdFive).Select(x => x.OptionName).FirstOrDefault();

            //選項名稱
            var SpecificationIdOne = db.ProductOptions.Where(x => x.OptionName == optionNameOne).Select(x => x.SpecificationId).FirstOrDefault();
            var SpecificationIdTwo = db.ProductOptions.Where(x => x.OptionName == optionNameTwo).Select(x => x.SpecificationId).FirstOrDefault();
            var SpecificationIdThree = db.ProductOptions.Where(x => x.OptionName == optionNameThree).Select(x => x.SpecificationId).FirstOrDefault();
            var SpecificationIdFour = db.ProductOptions.Where(x => x.OptionName == optionNameFour).Select(x => x.SpecificationId).FirstOrDefault();
            var SpecificationIdOFive = db.ProductOptions.Where(x => x.OptionName == optionNameFive).Select(x => x.SpecificationId).FirstOrDefault();

            var SpecificationNameOne=db.ProductSpecifications.Where(x=>x.SpecificationId== SpecificationIdOne).Select(x => x.SpecificationName).FirstOrDefault();
            var SpecificationNameTwo = db.ProductSpecifications.Where(x => x.SpecificationId == SpecificationIdTwo).Select(x => x.SpecificationName).FirstOrDefault();
            var SpecificationNameThree = db.ProductSpecifications.Where(x => x.SpecificationId == SpecificationIdThree).Select(x => x.SpecificationName).FirstOrDefault();
            var SpecificationNameFour = db.ProductSpecifications.Where(x => x.SpecificationId == SpecificationIdFour).Select(x => x.SpecificationName).FirstOrDefault();
            var SpecificationNameFive = db.ProductSpecifications.Where(x => x.SpecificationId == SpecificationIdOFive).Select(x => x.SpecificationName).FirstOrDefault();

            //大分類名稱
            var categoryId = db.Products.Where(x => x.ProductId == productId).Select(x => x.CategoryId).FirstOrDefault();
            var categoryName=db.ProductMainCategories.Where(x=>x.CategoryId== categoryId).Select(x => x.CategoryName).FirstOrDefault();

            //小分類名稱
            var subcategoryId=db.Products.Where(x=>x.ProductId== productId).Select(x=>x.SubcategoryId).FirstOrDefault();
            var subcategoryName=db.ProductSubCategories.Where(x=>x.SubcategoryId== subcategoryId).Select(x=>x.SubcategoryName).FirstOrDefault();

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
                SpecificationNameOne = SpecificationNameOne,
                SpecificationNameTwo = SpecificationNameTwo,
                SpecificationNameThree = SpecificationNameThree,
                SpecificationNameFour = SpecificationNameFour,
                SpecificationNameFive = SpecificationNameFive,

                OptionNameOne = optionNameOne,
                OptionNameTwo= optionNameTwo,
                OptionNameThree= optionNameThree,
                OptionNameFour= optionNameFour,
                OptionNameFive= optionNameFive,

                CategoryName= categoryName,
                SubcategoryName = subcategoryName,
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