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
        public async Task<IActionResult> Index()
        {
            var products = await _productApiCalls.GetAllProducts();
            if (products == null)
                return BadRequest();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categoryList = await _productApiCalls.GetCreateProduct();
            ViewBag.CategoryNames = new SelectList(categoryList, "Id", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel product)
        {
            if (ModelState.IsValid)
            {
                var result = await _productApiCalls.PostCreateProduct(product);

                if (result.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                ModelState.AddModelError(string.Empty, await result.Content.ReadAsStringAsync());
            }

            var categoryList = await _productApiCalls.GetCreateProduct();
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductUpdateViewModel UpdateProduct)
        {

            if (ModelState.IsValid)
            {
                var result = await _productApiCalls.PostEditProduct(UpdateProduct);

                if (result.IsSuccessStatusCode)
                    return RedirectToAction("Index");
                else
                    ModelState.AddModelError(string.Empty, await result.Content.ReadAsStringAsync());
            }
            var product = await _productApiCalls.GetEditProduct(UpdateProduct.Id);
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _productApiCalls.GetEditProduct(id);
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ProductUpdateViewModel productToBeDeleted)
        {
            var deleteResult  = await _productApiCalls.Delete(productToBeDeleted.Id);

            if(deleteResult.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError(string.Empty, await deleteResult.Content.ReadAsStringAsync());
            return View(productToBeDeleted);
        }
    }
}