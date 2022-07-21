using Microsoft.AspNetCore.Mvc;
using MagazineManagment.DTO.ViewModels;
using MagazineManagment.Web.ApiCalls;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;

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
            Task<HttpResponseMessage> result = null;

            if (ModelState.IsValid)
            {
                result = _productApiCalls.PostCreateProduct(product);

                
                if (result.IsCompletedSuccessfully)
                    return RedirectToAction("Index");

                var content = await result.Result.Content.ReadAsStringAsync();
                var errorMessage = content.Split(@"""").ToList();
                var errorMessageIndex = errorMessage.IndexOf("reasonPhrase") + 2;
                ModelState.AddModelError(string.Empty, errorMessage[errorMessageIndex]);
            }


            var categoryList = _productApiCalls.GetCreateProduct();
            ViewBag.CategoryNames = new SelectList(categoryList, "Id", "CategoryName");
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
