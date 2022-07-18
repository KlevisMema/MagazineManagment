using Microsoft.AspNetCore.Mvc;
using MagazineManagment.DTO.ViewModels;
using MagazineManagment.Web.ApiCalls;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagazineManagment.ClientApplication.Controllers
{
    public class ProductApiCallController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var products = ProductApiCalls.GetAllProducts("Product");

            return View(products);
        }

        [HttpGet]
        public  IActionResult Create()
        {
            var categoryList= ProductApiCalls.GetCreateProduct("CategoryNameOnly");
            ViewBag.CategoryNames = new SelectList(categoryList, "Id", "CategoryName");
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductCreateViewModel product)
        {
            var categoryList = ProductApiCalls.GetCreateProduct("CategoryNameOnly");
            ViewBag.CategoryNames = new SelectList(categoryList, "Id", "CategoryName");
            return View("Create");
        }
    }
}
