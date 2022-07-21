using MagazineManagment.BLL.RepositoryServices.ServiceInterfaces;
using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DAL.DataContext;
using MagazineManagment.DTO.DataTransferObjects;
using MagazineManagment.DAL.Models;
using MagazineManagment.DTO.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace MagazineManagment.BLL.Services
{
    public class ProductRepository : IProductRepository
    {


        private readonly ApplicationDbContext _context;
       
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get all products
        public async Task<IEnumerable<ProductViewModel>> GetAllProductsAsync()
        {
            var products = await _context.Products.Select(x => x.AsProductDto()).ToListAsync();
            return products;
        }

        //Get a single product by id
        public async Task<ResponseService<ProductViewModel>> GetProductAsync(Guid id)
        {
            var product = await _context.Products/*.Include(p => p.ProductCategory)*/.FirstOrDefaultAsync(x => x.Id == id);

            if (product is null)
            {
                return ResponseService<ProductViewModel>.NotFound("Product doesnt exists");
            }

            return ResponseService<ProductViewModel>.Ok(product.AsProductDto());
        }

        //Create a product
        public async Task<ResponseService<ProductViewModel>> CreateProductAsync(ProductCreateViewModelNoIFormFile product)
        {

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == product.ProductCategoryId);

            if (category is null)
            {
                return ResponseService<ProductViewModel>.NotFound("Category not found");
            }


            var checkIfSerialNrExists = await  _context.Products.AnyAsync(sNr => sNr.SerialNumber == product.SerialNumber.ToUpper());
            if (checkIfSerialNrExists)
            {
                return ResponseService<ProductViewModel>.ErrorMsg($"Serial number {product.SerialNumber} exists, please give another  serial number");
            }

            try
            {
                Product newProduct = new()
                {
                    ProductName = product.ProductName,
                    Price = product.Price,
                    CreatedOn = DateTime.Now,
                    ProductDescription = product.ProductDescription,
                    ProductCategoryId = product.ProductCategoryId,
                    Image = product.Image,
                    CreatedBy = product.CreatedBy,
                    CurrencyType = product.CurrencyType,
                    SerialNumber = product.SerialNumber.ToUpper(),
                    ProductInStock = product.ProductInStock
                };

                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();
                return ResponseService<ProductViewModel>.Ok(newProduct.AsProductDto());
            }
            catch (Exception ex)
            {
                return ResponseService<ProductViewModel>.ExceptioThrow(ex.Message);
            }

        }

        // Update a Product 
        public async Task<ResponseService<ProductViewModel>> UpdateProductAsync(ProductUpdateViewModel product)
        {
            var productToBeUpdated = await _context.Products.FirstOrDefaultAsync(c => c.Id == product.Id);

            if (productToBeUpdated == null)
            {
                return ResponseService<ProductViewModel>.NotFound("Product does not exists");
            }

            productToBeUpdated.ProductName = product.ProductName;
            productToBeUpdated.Price = product.Price;
            productToBeUpdated.CreatedOn = DateTime.Now;
            productToBeUpdated.ProductDescription = product.ProductDescription;
            productToBeUpdated.SerialNumber = product.SerialNumber;
            productToBeUpdated.CreatedBy = product.CreatedBy;
            //productToBeUpdated.Image = BaseArrayImage;
            productToBeUpdated.ProductInStock = product.ProductInStock;

            try
            {
                _context.Products.Update(productToBeUpdated);
                await _context.SaveChangesAsync();
                return ResponseService<ProductViewModel>.Ok(productToBeUpdated.AsProductDto());
            }
            catch (Exception ex)
            {
                return ResponseService<ProductViewModel>.ExceptioThrow(ex.Message);
            }

        }

        //Delete a product
        public async Task<ResponseService<ProductViewModel>> DeleteProductAsync(Guid id)
        {
            var productToBeDeleted = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (productToBeDeleted is null)
            {
                return ResponseService<ProductViewModel>.NotFound($"Produkti with id {id} does not excists !!");
            }
            try
            {
                _context.Products.Remove(productToBeDeleted);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                return ResponseService<ProductViewModel>.ExceptioThrow(ex.Message);
            }

            return ResponseService<ProductViewModel>.Deleted($"Produkti with id : {id} has been deleted!!!!");
        }

        // Display all products together with their category info
        public async Task<IEnumerable<ProductsAndCategoryInfoViewModel>> ProductsAndCategoryAsync()
        {
            return await _context.Products.Include(c => c.ProductCategory).Select(x => x.AsProductCategoryDto()).ToListAsync();
        }

        // Serach a product by its name
        public async Task<ResponseService<ProductViewModel>> GetProductByNameAsync(string ProductName)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductName == ProductName);
            if (product == null)
                return ResponseService<ProductViewModel>.NotFound("Produkti nuk egziston");
            return ResponseService<ProductViewModel>.Ok(product.AsProductDto());
        }

    }
}