using Microsoft.AspNetCore.Mvc;
using MagazineManagment.DTO.ViewModels;
using MagazineManagment.Web.ApiCalls;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace MagazineManagment.ClientApplication.Controllers
{
    public class ProductApiCallController : Controller
    {
        private readonly IProductApiCalls _productApiCalls;

        public ProductApiCallController(IProductApiCalls productApiCalls)
        {
            _productApiCalls = productApiCalls;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var products = _productApiCalls.GetAllProducts();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var categoryList = _productApiCalls.GetCreateProduct();
            ViewBag.CategoryNames = new SelectList(categoryList, "Id", "CategoryName");
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductCreateViewModel product)
        {
            //var categoryList = _productApiCalls.GetCreateProduct();
            //ViewBag.CategoryNames = new SelectList(categoryList, "Id", "CategoryName");
 
            return RedirectToAction("Index");
        }
    }
}
