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
            var result =  _productApiCalls.PostCreateProduct(product);
            if (result.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var categoryList = _productApiCalls.GetCreateProduct();
            ViewBag.CategoryNames = new SelectList(categoryList, "Id", "CategoryName");
            ModelState.AddModelError(string.Empty, result.Content.ToString());
            return View(product);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var product =  _productApiCalls.GetEdit(id);
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(ProductUpdateViewModel UpdateProduct)
        {
            var categoryList = _productApiCalls.GetCreateProduct();
            ViewBag.CategoryNames = new SelectList(categoryList, "Id", "CategoryName");
            
            _productApiCalls.PostEdit(UpdateProduct);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(Guid id)
        {
            _productApiCalls.Delete(id);
            return RedirectToAction("Index");
        }
    }
}   
