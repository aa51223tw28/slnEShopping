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
            return View();
        }

        public ActionResult SearchSubcategoryIdapi(string subcategoryName)
        {
            var db = new AppDbContext();
            var subcategoryId = db.ProductSubCategories.Where(x => x.SubcategoryName == subcategoryName).Select(x=>x.SubcategoryId).FirstOrDefault();
            return Json(subcategoryId, JsonRequestBehavior.AllowGet);
        }
    }
}