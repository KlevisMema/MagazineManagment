using FluentValidation;
using MagazineManagment.BLL.RepositoryServices.ServiceInterfaces;
using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DTO.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagazineManagment.Web.Controllers
{
    /// <summary>
    /// Category Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IValidator<CategoryCreateViewModel> _validator;

        /// <summary>
        /// Inject category service
        /// </summary>
        public CategoryController(ICategoryRepository categoryRepository, IValidator<CategoryCreateViewModel> validator)
        {
            _categoryRepository = categoryRepository;
            _validator = validator;
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<CategoryViewModel>>> GetAllCategories()
        {
            var getAllCategories = await _categoryRepository.GetAllCategoriesAsync();
            return Ok(getAllCategories);
        }

        /// <summary>
        ///  Get category by id
        /// </summary>
        /// <param name="id">The category id</param>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseService<CategoryViewModel>>> GetCategory(Guid id)
        {
            var result = await _categoryRepository.GetCategoryAsync(id);

            if (result.Success)
                return Ok(result.Value);

            return BadRequest(result);
        }

        /// <summary>
        ///  Create a  category
        /// </summary>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseService<CategoryViewModel>>> CreateCategory(CategoryCreateViewModel createCategory)
        {
            var resultValidator = await _validator.ValidateAsync(createCategory);

            if (!resultValidator.IsValid)
                return BadRequest(resultValidator.ToDictionary());

            var resultCreate = await _categoryRepository.CreateCategoryAsync(createCategory, HttpContext);
            if (resultCreate.Success)
                return Ok(resultCreate.Value);
            return BadRequest(resultCreate.Message);
        }

        /// <summary>
        /// Update a category
        /// </summary>
        /// <param name="category"></param>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseService<CategoryViewModel>>> UpdateCategory(CategoryUpdateViewModel category)
        {
            var result = await _categoryRepository.UpdateCategoryAsync(category, HttpContext);

            if (result.Success)
                return Ok(result.Value);

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Delete a category
        /// </summary>
        /// <param name="id"></param>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseService<CategoryViewModel>>> DeleteCategory(Guid id)
        {
            var resultDelete = await _categoryRepository.DeleteCategoryAsync(id);

            if (resultDelete.Success)
                return Ok(resultDelete.Message);

            return BadRequest(resultDelete.Message);

        }

        /// <summary>
        /// Get category names
        /// </summary>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("CategoryNameOnly")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryNameOnlyViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CategoryNameOnlyViewModel>> GetNamesOnlyCategories()
        {
            var result = await _categoryRepository.GetNamesOnlyCategories();
            return Ok(result);
        }
    }
}