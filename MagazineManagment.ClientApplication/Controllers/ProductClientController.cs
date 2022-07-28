using Microsoft.AspNetCore.Mvc;
using MagazineManagment.DTO.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces;
using System.Security.Claims;

namespace MagazineManagment.ClientApplication.Controllers
{
    public class ProductClientController : Controller
    {

        private readonly IProductApiCalls _productApiCalls;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string GetIdentityUserName()
        {
            var identityUser = HttpContext.User.Identity as ClaimsIdentity;
            var userName = identityUser.FindFirst(ClaimTypes.Name).Value;
            return userName;
        }

        public ProductClientController(IProductApiCalls productApiCalls, IHttpContextAccessor httpContextAccessor)
        {
            _productApiCalls = productApiCalls;
            _httpContextAccessor = httpContextAccessor;
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
            var deleteResult = await _productApiCalls.Delete(productToBeDeleted.Id);

            if (deleteResult.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError(string.Empty, await deleteResult.Content.ReadAsStringAsync());
            return View(productToBeDeleted);
        }
    }
}