﻿using prjEShopping.Models.DTOs;
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
            List<UserProductIndexDto> datas;
            datas = db.Products.ToList()
                                .Select(x =>
                                {
                                    return new UserProductIndexDto()
                                    {
                                        ProductId = x.ProductId,
                                        ProductName = x.ProductName,
                                        ProductDescription = x.ProductDescription,
                                        Price = (decimal)x.Price,
                                        ProductImagePathOne = x.ProductImagePathOne,
                                        ProductImagePathTwo = x.ProductImagePathTwo,
                                        ProductImagePathThree = x.ProductImagePathThree,
                                    };
                                }).ToList();

            return View(datas);
        }

        public ActionResult UserSingleProduct(int productId)
        {            
            var db=new AppDbContext();
            var product = db.Products.FirstOrDefault(x => x.ProductId == productId);            

            var datas = new UserProductIndexDto()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Price = (decimal)product.Price,
                ProductImagePathOne = product.ProductImagePathOne,
                ProductImagePathTwo = product.ProductImagePathTwo,
                ProductImagePathThree = product.ProductImagePathThree
            };

            return View(datas);
        }
    }
}