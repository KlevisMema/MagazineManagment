using MagazineManagment.BLL.RepositoryServices.ServiceInterfaces;
using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DTO.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagazineManagment.Web.Controllers
{
    /// <summary>
    /// Product controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        /// <summary>
        /// Inject product service  interface
        /// </summary>
        /// <param name="productRepository"></param>
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin,Employee")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetAllProductsAsync()
        {
            var allProducts = await _productRepository.GetAllProductsAsync();
            return Ok(allProducts);
        }

        /// <summary>
        /// Get a product by id 
        /// </summary>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseService<ProductViewModel>>> GetProductAsync(Guid id)
        {
            var resultGetProdut = await _productRepository.GetProductAsync(id);

            if (resultGetProdut.Success)
                return Ok(resultGetProdut.Value);

            return BadRequest(resultGetProdut);
        }

        /// <summary>
        /// Create a product
        /// </summary>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseService<ProductViewModel>>> CreateProductAsync(ProductCreateViewModelNoIFormFile product)
        {
            var resultCreateProduct = await _productRepository.CreateProductAsync(product,HttpContext);

            if (resultCreateProduct.Success)
                return Ok(resultCreateProduct);

            return BadRequest(resultCreateProduct.Message);
        }

        /// <summary>
        /// Update a product
        /// </summary>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin,Employee")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductPostEditViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseService<ProductPostEditViewModel>>> UpdateProductAsync(ProductPostEditViewModel product)
        {
            var resultUpdateProduct = await _productRepository.UpdateProductAsync(product,HttpContext);

            if (resultUpdateProduct.Success)
                return Ok(resultUpdateProduct.Value);

            return BadRequest(resultUpdateProduct.Message);
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin,Employee")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResponseService<ProductViewModel>>> DeleteProductAsync(Guid id)
        {
            var productToBeDeleted = await _productRepository.DeleteProductAsync(id);

            return Ok(productToBeDeleted);
        }

        /// <summary>
        /// Get full info about a product 
        /// </summary>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin")]
        [HttpGet("GetProductsAndCategory")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductsAndCategoryInfoViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<ProductsAndCategoryInfoViewModel>>> ProductsAndCategoryAsync()
        {
            var getProductWithCategoryIncluded = await _productRepository.ProductsAndCategoryAsync();
            return Ok(getProductWithCategoryIncluded);
        }

        /// <summary>
        /// Get a product by name
        /// </summary>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("GetProductByName/{productName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetPorductByNameAsync([FromRoute] string productName)
        {
            var resultGetProductByItsName = await _productRepository.GetProductByNameAsync(productName);
            return Ok(resultGetProductByItsName);
        }

        /// <summary>
        /// Get product image 
        /// </summary>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("GetProductImage/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductImageOnly))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseService<ProductImageOnly>>> GetProductImageOnlyAsync(Guid id)
        {
            var getProductImage = await _productRepository.GetProductImage(id);

            if (getProductImage.Success)
                return Ok(getProductImage.Value);
            return BadRequest(getProductImage);
        }

        /// <summary>
        /// Get all products changes made by employees
        /// </summary>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin")]
        [HttpGet("GetProducChangesByEmpolyees")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductsRecordCopyViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<ProductsRecordCopyViewModel>>> GetProducChangesByEmpolyees()
        {
            var changesByEmployees = await _productRepository.GetProducChangesByEmpolyees();
            return Ok(changesByEmployees);
        }

        /// <summary>
        /// Delete the changes that employye made 
        /// </summary>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteProducChangesByEmpolyees/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductsRecordCopyViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseService<ProductsRecordCopyViewModel>>> DeleteProductChangeByEmployee(Guid id)
        {
            var productToBeDeleted = await _productRepository.DeleteProductChangeByEmployee(id);

            if (productToBeDeleted.Success)
                return Ok(productToBeDeleted);

            return BadRequest(productToBeDeleted);
        }
    }
}