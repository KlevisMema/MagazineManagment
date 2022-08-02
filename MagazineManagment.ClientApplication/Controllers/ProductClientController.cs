using Microsoft.AspNetCore.Mvc;
using MagazineManagment.DTO.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MagazineManagment.ClientApplication.Controllers
{
    public class ProductClientController : Controller
    {

        private readonly IProductApiCalls _productApiCalls;

        private string GetIdentityUserName()
        {
            var identityUser = HttpContext.User.Identity as ClaimsIdentity;
            var userName = identityUser.FindFirst(ClaimTypes.Name).Value;
            return userName;
        }

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel product)
        {
            if (ModelState.IsValid)
            {
                var result = await _productApiCalls.PostCreateProduct(product, HttpContext);
                if (result.IsSuccessStatusCode)
                    return RedirectToAction("Index");
                ModelState.AddModelError(string.Empty, await result.Content.ReadAsStringAsync());
            }
            var categoryList = await _productApiCalls.GetCreateProduct();
            ViewBag.CategoryNames = new SelectList(categoryList, "Id", "CategoryName");
            return View(product);
        }

        [Authorize(Roles = "Admin,Employee")]
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

        [Authorize(Roles = "Admin,Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductUpdateViewModel UpdateProduct)
        {
            if (ModelState.IsValid)
            {
                var userName = GetIdentityUserName();
                UpdateProduct.UserName = userName;

                var result = await _productApiCalls.PostEditProduct(UpdateProduct);

                if (result.IsSuccessStatusCode)
                    return RedirectToAction("Index");
                else
                    ModelState.AddModelError(string.Empty, await result.Content.ReadAsStringAsync());
            }
            var product = await _productApiCalls.GetEditProduct(UpdateProduct.Id);
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _productApiCalls.GetEditProduct(id);
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ProductUpdateViewModel productToBeDeleted)
        {
            var deleteResult = await _productApiCalls.Delete(productToBeDeleted.Id);

            if (deleteResult.IsSuccessStatusCode)
                return RedirectToAction("Index");

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
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}