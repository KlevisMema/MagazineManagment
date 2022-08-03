using MagazineManagment.DTO.ViewModels;
using MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagazineManagment.ClientApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryClientController : Controller
    {
        private readonly ICategoryApiCalls _categoryApiCalls;
        public CategoryClientController(ICategoryApiCalls categoryApiCalls)
        {
            _categoryApiCalls = categoryApiCalls;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryApiCalls.GetAllCategories();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateViewModel category)
        {
            if (ModelState.IsValid)
            {
                var postResult = await _categoryApiCalls.PostCreateCategory(category, HttpContext);

                if (postResult.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                ModelState.AddModelError(string.Empty, await postResult.Content.ReadAsStringAsync());
            }
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var category = await _categoryApiCalls.GetEditCategory(id);

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryUpdateViewModel categoryUpdate)
        {
            if (ModelState.IsValid)
            {
                var editResult = await _categoryApiCalls.PostEditCategory(categoryUpdate, HttpContext);

                if (editResult.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                ModelState.AddModelError(string.Empty, await editResult.Content.ReadAsStringAsync());
            }

            return View(categoryUpdate);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var category = await _categoryApiCalls.GetEditCategory(id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(CategoryUpdateViewModel category)
        {
            var deleteResult = await _categoryApiCalls.PostDeleteCategory(category.Id);
            if (deleteResult.IsSuccessStatusCode)
                return RedirectToAction("index");

            ModelState.AddModelError(string.Empty, await deleteResult.Content.ReadAsStringAsync());
            return View(category);
        }
    }
}
