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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel product)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage result = null;
                result = await _productApiCalls.PostCreateProduct(product);

                if (result.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                ModelState.AddModelError(string.Empty,await result.Content.ReadAsStringAsync());
            }

            var categoryList = _productApiCalls.GetCreateProduct();
            ViewBag.CategoryNames = new SelectList(categoryList, "Id", "CategoryName");
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await _productApiCalls.GetEditProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductUpdateViewModel UpdateProduct)
        {

            if (ModelState.IsValid)
            {
                var  result  = await _productApiCalls.PostEditProduct(UpdateProduct);

                if (result.IsSuccessStatusCode)
                    return RedirectToAction("Index");
                else
                    ModelState.AddModelError(string.Empty, await result.Content.ReadAsStringAsync());
            }
            var product = await _productApiCalls.GetEditProduct(UpdateProduct.Id);
            return View(product);
        }

        public IActionResult Delete(Guid id)
        {
            _productApiCalls.Delete(id);
            return RedirectToAction("Index");
        }
    }
}   