using MagazineManagment.DTO.ViewModels;
using MagazineManagmet.ApiCalls.ApiCalls;
using Microsoft.AspNetCore.Mvc;

namespace MagazineManagment.ClientApplication.Controllers
{
    public class CategoryApiCallController : Controller
    {
        private readonly ICategoryApiCalls _categoryApiCalls;
        public CategoryApiCallController(ICategoryApiCalls categoryApiCalls)
        {
            _categoryApiCalls = categoryApiCalls;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories =  await _categoryApiCalls.GetAllCategories();
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
                var postCategoryRespond = await _categoryApiCalls.CreateCategory(category);

                if (postCategoryRespond.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                ModelState.AddModelError(string.Empty , await postCategoryRespond.Content.ReadAsStringAsync());
            }
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _categoryApiCalls.EditCategory(id);
            
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryUpdateViewModel categoryUpdate)
        {
            return View();
        }

    }
}
