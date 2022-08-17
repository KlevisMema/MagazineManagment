using Microsoft.AspNetCore.Mvc;
using MagazineManagment.DTO.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces;
using Microsoft.AspNetCore.Authorization;
using FormHelper;

namespace MagazineManagment.ClientApplication.Controllers
{
    public class ProductClientController : Controller
    {

        private readonly IProductApiCalls _productApiCalls;

        public ProductClientController(IProductApiCalls productApiCalls)
        {
            _productApiCalls = productApiCalls;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productApiCalls.GetAllProducts();

            if (products == null)
                return BadRequest();

            return View(products);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categoryList = await _productApiCalls.GetCreateProduct();
            ViewBag.CategoryNames = new SelectList(categoryList, "Id", "CategoryName");

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, FormValidator]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel product)
        {
            var result = await _productApiCalls.PostCreateProduct(product);

            if (result.IsSuccessStatusCode)
                return FormResult.CreateSuccessResult("Product created successfully", Url.Action("Index", 1000));

            ModelState.AddModelError(string.Empty, await result.Content.ReadAsStringAsync());
            var categoryList = await _productApiCalls.GetCreateProduct();
            ViewBag.CategoryNames = new SelectList(categoryList, "Id", "CategoryName");
            return View(product);
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await _productApiCalls.GetEditProduct(id);

            if (product.ProductName is null)
                return NotFound();

            var categoryList = await _productApiCalls.GetCreateProduct();
            ViewBag.CategoryNames = new SelectList(categoryList, "Id", "CategoryName");
            return View(product);
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpPost, FormValidator]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductUpdateViewModel UpdateProduct)
        {
            var result = await _productApiCalls.PostEditProduct(UpdateProduct);
            if (result.IsSuccessStatusCode)
                return FormResult.CreateSuccessResult("Product edited successfully", Url.Action("Index", 1000));

            var errorMsg = await result.Content.ReadAsStringAsync();
            return FormResult.CreateWarningResult(errorMsg);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _productApiCalls.GetEditProduct(id);

            if (product.ProductName is null)
                return NotFound();

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ProductUpdateViewModel productToBeDeleted)
        {
            var deleteResult = await _productApiCalls.Delete(productToBeDeleted.Id);

            if (deleteResult.IsSuccessStatusCode)
                return FormResult.CreateSuccessResult("Product deleted successfully", Url.Action("Index", 1000));

            ModelState.AddModelError(string.Empty, await deleteResult.Content.ReadAsStringAsync());
            return View(productToBeDeleted);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetChangesMadeByEmployees()
        {
            var productChanges = await _productApiCalls.GetProductChangesByEmpolyees();

            return View(productChanges);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProductChangeByEmployee(Guid id)
        {
            var deleteResult = await _productApiCalls.DeleteProductChangeByEmployee(id);

            if (deleteResult.IsSuccessStatusCode)
                return RedirectToAction("GetChangesMadeByEmployees");

            ModelState.AddModelError(string.Empty, await deleteResult.Content.ReadAsStringAsync());
            return RedirectToAction("GetChangesMadeByEmployees");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> DetailsOfProductChangedByEmployee(Guid id)
        {
            var product = await _productApiCalls.DetailsOfProductChangedByEmployee(id);

            if (product.ProductName is null)
                return NotFound();

            return View(product);
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> SearchProduct(string productName)
        {
            var productSearch = await _productApiCalls.SearchProduct(productName);
            return View("Index",productSearch);
        }
    }
}