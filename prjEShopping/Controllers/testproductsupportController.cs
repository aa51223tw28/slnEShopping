using prjEShopping.Models.DTOs;
using prjEShopping.Models.EFModels;
using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace prjEShopping.Controllers
{
    public class testproductsupportController : Controller
    {
        // GET: testproductsupport
        public ActionResult UserSingleProduct2(int productId)
        {
            var customerAccount = User.Identity.Name;

            var db = new AppDbContext();
            var userid = db.Users.Where(x => x.UserAccount == customerAccount).Select(x => x.UserId).FirstOrDefault();
            var cartid = db.ShoppingCarts.Where(x => x.UserId == userid).OrderByDescending(x => x.CartId).Select(x => x.CartId).FirstOrDefault();

            var product = db.Products.FirstOrDefault(x => x.ProductId == productId);

            //剩餘多少數量計算式為ProductStocks table=StockQuantity-OrderQuantity
            var orderQuantity = db.ProductStocks.Where(x => x.ProductId == productId).Select(x => x.OrderQuantity).FirstOrDefault() ?? 0;
            var stockQuantity = db.ProductStocks.Where(x => x.ProductId == productId).Select(x => x.StockQuantity).FirstOrDefault() ?? 0;

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
            var optionNameTwo = db.ProductOptions.Where(x => x.OptionId == parsedOptionIdTwo).Select(x => x.OptionName).FirstOrDefault();
            var optionNameThree = db.ProductOptions.Where(x => x.OptionId == parsedOptionIdThree).Select(x => x.OptionName).FirstOrDefault();
            var optionNameFour = db.ProductOptions.Where(x => x.OptionId == parsedOptionIdFour).Select(x => x.OptionName).FirstOrDefault();
            var optionNameFive = db.ProductOptions.Where(x => x.OptionId == parsedOptionIdFive).Select(x => x.OptionName).FirstOrDefault();

            //選項名稱
            var SpecificationIdOne = db.ProductOptions.Where(x => x.OptionName == optionNameOne).Select(x => x.SpecificationId).FirstOrDefault();
            var SpecificationIdTwo = db.ProductOptions.Where(x => x.OptionName == optionNameTwo).Select(x => x.SpecificationId).FirstOrDefault();
            var SpecificationIdThree = db.ProductOptions.Where(x => x.OptionName == optionNameThree).Select(x => x.SpecificationId).FirstOrDefault();
            var SpecificationIdFour = db.ProductOptions.Where(x => x.OptionName == optionNameFour).Select(x => x.SpecificationId).FirstOrDefault();
            var SpecificationIdOFive = db.ProductOptions.Where(x => x.OptionName == optionNameFive).Select(x => x.SpecificationId).FirstOrDefault();

            var SpecificationNameOne = db.ProductSpecifications.Where(x => x.SpecificationId == SpecificationIdOne).Select(x => x.SpecificationName).FirstOrDefault();
            var SpecificationNameTwo = db.ProductSpecifications.Where(x => x.SpecificationId == SpecificationIdTwo).Select(x => x.SpecificationName).FirstOrDefault();
            var SpecificationNameThree = db.ProductSpecifications.Where(x => x.SpecificationId == SpecificationIdThree).Select(x => x.SpecificationName).FirstOrDefault();
            var SpecificationNameFour = db.ProductSpecifications.Where(x => x.SpecificationId == SpecificationIdFour).Select(x => x.SpecificationName).FirstOrDefault();
            var SpecificationNameFive = db.ProductSpecifications.Where(x => x.SpecificationId == SpecificationIdOFive).Select(x => x.SpecificationName).FirstOrDefault();

            //大分類名稱
            var categoryId = db.Products.Where(x => x.ProductId == productId).Select(x => x.CategoryId).FirstOrDefault();
            var categoryName = db.ProductMainCategories.Where(x => x.CategoryId == categoryId).Select(x => x.CategoryName).FirstOrDefault();

            //小分類名稱
            var subcategoryId = db.Products.Where(x => x.ProductId == productId).Select(x => x.SubcategoryId).FirstOrDefault();
            var subcategoryName = db.ProductSubCategories.Where(x => x.SubcategoryId == subcategoryId).Select(x => x.SubcategoryName).FirstOrDefault();

            var datas = new UserProductIndexDto()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Price = (decimal)product.Price,
                ProductImagePathOne = product.ProductImagePathOne,
                ProductImagePathTwo = product.ProductImagePathTwo,
                ProductImagePathThree = product.ProductImagePathThree,
                ProductStock = stockQuantity - orderQuantity,
                QuantitySold = (int)quantitySold,

                //一堆規格
                BrandName = brandName,
                SpecificationNameOne = SpecificationNameOne,
                SpecificationNameTwo = SpecificationNameTwo,
                SpecificationNameThree = SpecificationNameThree,
                SpecificationNameFour = SpecificationNameFour,
                SpecificationNameFive = SpecificationNameFive,

                OptionNameOne = optionNameOne,
                OptionNameTwo = optionNameTwo,
                OptionNameThree = optionNameThree,
                OptionNameFour = optionNameFour,
                OptionNameFive = optionNameFive,

                CategoryName = categoryName,
                SubcategoryName = subcategoryName,
            };
            //現在購物車該使用者該購物車id該商品的數量
            var qua = db.ShoppingCartDetails.Where(x => x.CartId == cartid && x.ProductId == productId).Select(x => x.Quantity).FirstOrDefault();
            if (qua != null)
            {
                ViewBag.TotalQuantity = qua;
            }
            else
            {
                ViewBag.TotalQuantity = 0;
            }
            ViewBag.SupportNum = GenerateSupportNumberU();
            ViewBag.UserId = userid;
            return View(datas);
        }

        [HttpPost]
        //客戶端寄信
        public ActionResult CSSendMail(SupportVM vm)
        {
            var db = new AppDbContext();
            if (vm != null)
            {   //存圖..
                imageAdd(vm);
                var s = new Support();
                s = SupportChange.VM2Support(vm);
                db.Supports.Add(s);
                db.SaveChanges();
                return new EmptyResult();
            }
            return View(vm);
        }

        //user 寄送
        public string GenerateSupportNumberU()
        {
            var db = new AppDbContext();
            var today = DateTime.Now;
            // 獲取當前日期的格式化字符串 "230812"
            var datePart = today.ToString("yyMMdd");

            // 查詢當天已有多少條記錄
            var countForToday = db.Supports
                                  .Where(s => s.SupportNumber.StartsWith("US" + datePart))
                                  .Count();
            // 生成下一個編號
            var numberPart = (countForToday + 1).ToString("D4"); // 四位數，不足前面補0

            return "US" + datePart + numberPart;
        }

        //存圖範例
        private void imageAdd(SupportVM vm)
        {
            if (vm.ImageFile != null && vm.ImageFile.ContentLength > 0)
            {
                // 生成一個唯一的檔案名稱
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.ImageFile.FileName);

                // 指定檔案的保存路徑
                var path = Path.Combine(Server.MapPath("~/img/"), fileName);

                // 儲存檔案
                vm.ImageFile.SaveAs(path);

                // 將檔案名稱保存到Support模型的相應欄位
                vm.ImageLink = fileName;
            }
        }

        public static class SupportChange
        {
            public static SupportVM Support2VM(Support s)
            {
                return new SupportVM
                {
                    SupportId = s.SupportId,
                    SupportNumber = s.SupportNumber,
                    AdminId = s.AdminId,
                    SellerId = s.SellerId,
                    UserId = s.UserId,
                    ProductId = s.ProductId,
                    SupportType = s.SupportType,
                    SupportTitle = s.SupportTitle,
                    SupportText = s.SupportText,
                    ReceivedTime = s.ReceivedTime,
                    SupportStatus = s.SupportStatus,
                    ImageLink = s.ImageLink,
                };
            }
            public static List<SupportVM> Support2VM(this IEnumerable<Support> source)
            {
                if (source == null || source.Count() == 0)
                {
                    return Enumerable.Empty<SupportVM>().ToList();
                }
                return source.Select(s => Support2VM(s)).ToList();
            }
            public static Support VM2Support(SupportVM vm)
            {
                return new Support
                {
                    SupportId = vm.SupportId,
                    SupportNumber = vm.SupportNumber,
                    AdminId = vm.AdminId,
                    SellerId = vm.SellerId,
                    UserId = vm.UserId,
                    ProductId = vm.ProductId,
                    SupportType = vm.SupportType,
                    SupportTitle = vm.SupportTitle,
                    SupportText = vm.SupportText,
                    ReceivedTime = vm.ReceivedTime,
                    SupportStatus = vm.SupportStatus,
                    ImageLink = vm.ImageLink,
                };
            }

        }

        public partial class SupportVM
        {
            public int SupportId { get; set; }

            [DisplayName("信件編號")]
            [StringLength(50)]
            public string SupportNumber { get; set; }

            [DisplayName("管理員ID")]
            public int? AdminId { get; set; }

            [DisplayName("商家ID")]
            public int? SellerId { get; set; }

            [DisplayName("會員ID")]
            public int? UserId { get; set; }

            [DisplayName("商品ID")]
            public int? ProductId { get; set; }

            [DisplayName("信件類型")]
            [StringLength(50)]
            public string SupportType { get; set; }

            [DisplayName("標題")]
            [StringLength(50)]
            public string SupportTitle { get; set; }

            [DisplayName("內容")]
            [StringLength(4000)]
            public string SupportText { get; set; }

            [DisplayName("發信時間")]
            public DateTime? ReceivedTime { get; set; }

            [DisplayName("信件狀態")]
            [StringLength(50)]
            public string SupportStatus { get; set; }

            [DisplayName("上傳圖片")]
            [StringLength(100)]
            public string ImageLink { get; set; }

            public HttpPostedFileBase ImageFile { get; set; }
        }
    }
}