using MagazineManagment.BLL.RepositoryServices.ServiceInterfaces;
using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DTO.ViewModels;
using MagazineManagment.Shared.CustomAuthorizationFilter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagazineManagment.Web.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // Get all categories
        [Auth]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryViewModel>>> GetAllCategories()
        {
            var getAllCategories = await _categoryRepository.GetAllCategoriesAsync();
            return Ok(getAllCategories);
        }

        // Get a single category by id
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseService<CategoryViewModel>>> GetCategory(Guid id)
        {
            var result = await _categoryRepository.GetCategoryAsync(id);

            if (result.Success)
                return Ok(result.Value);

            return BadRequest(result.Message);
        }

        // Create a category
        [HttpPost]
        public async Task<ActionResult<ResponseService<CategoryViewModel>>> CreateCategory(CategoryCreateViewModel createCategory)
        {
            var resultCreate = await _categoryRepository.CreateCategoryAsync(createCategory);
            if (resultCreate.Success)
                return Ok(resultCreate.Value);
            return BadRequest(resultCreate.Message);
        }

        // Update a category
        [HttpPut]
        public async Task<ActionResult<ResponseService<CategoryViewModel>>> UpdateCategory(CategoryUpdateViewModel category)
        {
            var result = await _categoryRepository.UpdateCategoryAsync(category);

            if (result.Success)
                return Ok(result.Value);

            return BadRequest(result.Message);
        }

        // Delete a category
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseService<CategoryViewModel>>> DeleteCategory(Guid id)
        {
            var resultDelete = await _categoryRepository.DeleteCategoryAsync(id);

            if (resultDelete.Success)
                return Ok(resultDelete.Message);

            return BadRequest(resultDelete.Message);

        }

        [HttpGet("CategoryNameOnly")]
        public async Task<ActionResult<CategoryNameOnlyViewModel>> GetNamesOnlyCategories()
        {
            var result = await _categoryRepository.GetNamesOnlyCategories();
            return Ok(result);
        }

    }
}
