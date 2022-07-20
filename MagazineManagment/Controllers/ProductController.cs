﻿using MagazineManagment.BLL.RepositoryServices.ServiceInterfaces;
using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DTO.ViewModels;
using Microsoft.AspNetCore.Mvc;



namespace MagazineManagment.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Get all products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetAllProductsAsync()
        {
            var allProducts = await _productRepository.GetAllProductsAsync();

            return Ok(allProducts);
        }

        // Get a single product with id
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseService<ProductsAndCategoryInfoViewModel>>> GetProductAsync(Guid id)
        {
            var resultGetProdut = await _productRepository.GetProductAsync(id);

            if (resultGetProdut.Success)
                return Ok(resultGetProdut.Value);

            return BadRequest(resultGetProdut.ErrorMessage);
        }

        // Create a new product
        [HttpPost]
        public async Task<ActionResult<ResponseService<ProductViewModel>>> CreateProductAsync(ProductCreateViewModelNoIFormFile product)
        {
            var resultCreateProduct = await _productRepository.CreateProductAsync(product);

            if (resultCreateProduct.Success)
                return Ok(resultCreateProduct.Value);

            else
                return BadRequest(resultCreateProduct.ErrorMessage);
        }

        // Update Product 
        [HttpPut]
        public async Task<ActionResult<ResponseService<ProductViewModel>>> UpdateProductAsync([FromForm] ProductUpdateViewModel product)
        {
            var resultUpdateProduct = await _productRepository.UpdateProductAsync(product);

            if (resultUpdateProduct.Success)
                return Ok(resultUpdateProduct.Value);

            return BadRequest(resultUpdateProduct.ErrorMessage);
        }

        // Delete a product
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseService<ProductViewModel>>> DeleteProductAsync(Guid id)
        {
            var productToBeDeleted = await _productRepository.DeleteProductAsync(id);

            return Ok(productToBeDeleted.ErrorMessage);
        }

        // Display all products together with their category info
        [HttpGet("GetProductsAndCategory")]
        public async Task<ActionResult<IEnumerable<ProductsAndCategoryInfoViewModel>>> ProductsAndCategoryAsync()
        {
            var getProductWithCategoryIncluded = await _productRepository.ProductsAndCategoryAsync();
            return Ok(getProductWithCategoryIncluded);
        }

        // serach a product by its name
        [HttpGet("GetProductByName/{productName}")]
        public async Task<ActionResult<ResponseService<ProductViewModel>>> GetPorductByNameAsync(string productName)
        {
            var resultGetProductByItsName = await _productRepository.GetProductByNameAsync(productName);

            if (resultGetProductByItsName.Success)
                return Ok(resultGetProductByItsName.Value);

            return BadRequest(resultGetProductByItsName.ErrorMessage);
        }

    }
}
