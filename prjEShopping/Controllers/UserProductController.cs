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
            List<Product> product = db.Products.ToList();
            return View(product);
        }

        public ActionResult UserSingleProduct(int productId)
        {            
            var db=new AppDbContext();
            List<UserProductIndexDto> datas;
            datas=db.Products.ToList()
                                .Select(x =>
                                {
                                    return new UserProductIndexDto()
                                    {
                                        ProductId = productId,
                                        ProductName=x.ProductName,
                                        ProductDescription=x.ProductDescription,
                                        Price=(decimal)x.Price,
                                        ProductImagePathOne=x.ProductImagePathOne,
                                        ProductImagePathTwo=x.ProductImagePathTwo,
                                        ProductImagePathThree=x.ProductImagePathThree,
                                    };
                                }).ToList();
            return View(datas);
        }
    }
}