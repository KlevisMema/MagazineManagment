using MagazineManagment.BLL.RepositoryServices.ServiceInterfaces;
using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DAL.DataContext;
using MagazineManagment.DAL.Models;
using MagazineManagment.DTO.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MagazineManagment.Shared.UsersSeedValues;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using MagazineManagment.BLL.RepositoryServices.GenericService;

namespace MagazineManagment.BLL.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _user;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductViewModel, Product, Product> _createProduct;
        private readonly IGenericRepository<ProductPostEditViewModel, ProductPostEditViewModel, Product> _updateProduct;


        public ProductRepository(ApplicationDbContext context, UserManager<IdentityUser> user,
                                 IMapper mapper, IGenericRepository<ProductViewModel, Product, Product> createProduct,
                                 IGenericRepository<ProductPostEditViewModel, ProductPostEditViewModel, Product> updateProduct)
        {
            _context = context;
            _user = user;
            _mapper = mapper;
            _createProduct = createProduct;
            _updateProduct = updateProduct; ;
        }

        //Get user name
        private static string GetUser(HttpContext context)
        {
            var headers = context.Request.Headers["Authorization"].ToString().Remove(0, 7);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(headers);
            var getUsername = token.Claims.Where(t => t.Type == "email").Select(v => v.Value).ToArray();

            return getUsername[0].ToString();
        }

        //Get all products
        public async Task<IEnumerable<ProductViewModel>> GetAllProductsAsync()
        {
            var products = await _context.Products.Include(p => p.ProductCategory).ToListAsync();
            return _mapper.Map<IEnumerable<ProductViewModel>>(products);
        }

        //Get a single product by id
        public async Task<ResponseService<ProductViewModel>> GetProductAsync(Guid id)
        {
            try
            {
                var product = await _context.Products.Include(p => p.ProductCategory).FirstOrDefaultAsync(x => x.Id == id);

                if (product is null)
                    return ResponseService<ProductViewModel>.NotFound("Product doesnt exists");
                return ResponseService<ProductViewModel>.Ok(_mapper.Map<ProductViewModel>(product));
            }
            catch (Exception ex)
            {
                return ResponseService<ProductViewModel>.ExceptioThrow(ex.Message);
            }
        }

        //Create a product
        public async Task<ResponseService<ProductViewModel>> CreateProductAsync(ProductCreateViewModelNoIFormFile product, HttpContext context)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == product.ProductCategoryId);

                if (category is null)
                    return ResponseService<ProductViewModel>.NotFound("Category not found");

                if (category.IsDeleted)
                    return ResponseService<ProductViewModel>.ErrorMsg($"Category {product.ProductCategoryId} doesn't exists, please give another category");

                var checkIfSerialNrExists = await _context.Products.AnyAsync(sNr => sNr.SerialNumber == product.SerialNumber.ToUpper());

                if (checkIfSerialNrExists)
                    return ResponseService<ProductViewModel>.ErrorMsg($"Serial number {product.SerialNumber} exists, please give another  serial number");

                product.CreatedBy = GetUser(context);
                return await _createProduct.Create(_mapper.Map<Product>(product));
            }
            catch (Exception ex)
            {
                return ResponseService<ProductViewModel>.ExceptioThrow(ex.Message);
            }
        }

        // Update a Product 
        public async Task<ResponseService<ProductPostEditViewModel>> UpdateProductAsync(ProductPostEditViewModel product, HttpContext context)
        {
            try
            {
                var productToBeUpdated = await _context.Products.FirstOrDefaultAsync(c => c.Id == product.Id);
                if (productToBeUpdated == null)
                    return ResponseService<ProductPostEditViewModel>.NotFound("Product does not exists");

                //  check if the serial number is changed, if yes  then check if does exists 
                bool ckeckIfExists = false;
                if (productToBeUpdated.SerialNumber != product.SerialNumber)
                    ckeckIfExists = await _context.Products.AnyAsync(s => s.SerialNumber == product.SerialNumber);
                if (ckeckIfExists)
                    return ResponseService<ProductPostEditViewModel>.ErrorMsg($"Serial number {product.SerialNumber} exists, please give another  serial number");

                // find  the user by email
                var findUser = await _user.FindByEmailAsync(GetUser(context));
                if (findUser == null)
                    return ResponseService<ProductPostEditViewModel>.NotFound("User not found");

                // find the role of the user
                var role = await _user.GetRolesAsync(findUser);
                if (role == null)
                    return ResponseService<ProductPostEditViewModel>.NotFound("Role not found");

                // if empolyee role then check how many products is he removing
                bool recordChangedByEmployee = false;
                if (role.Count == 1 && product.ProductInStock > productToBeUpdated.ProductInStock && role.Contains(RoleName.Employee))
                    return ResponseService<ProductPostEditViewModel>.ErrorMsg("You can't insert products in quantity");

                var amountOfProductsRemovedFromMagazine = product.ProductInStock - productToBeUpdated.ProductInStock;
                if (role.Contains(RoleName.Employee) && amountOfProductsRemovedFromMagazine < -20)
                    return ResponseService<ProductPostEditViewModel>.ErrorMsg("You can not remove more than 20 products ");
                else if (role.Contains(RoleName.Employee) && amountOfProductsRemovedFromMagazine == 0)
                    recordChangedByEmployee = false;
                else if (role.Contains(RoleName.Employee) && amountOfProductsRemovedFromMagazine > -20)
                    recordChangedByEmployee = true;

                productToBeUpdated.ProductInStock = product.ProductInStock;
                _mapper.Map(product, productToBeUpdated);
                var resultUpdate = await _updateProduct.Update(product, productToBeUpdated);

                if (recordChangedByEmployee)
                {
                    ProductRecordsChanged copyOfRecord = new()
                    {
                        ProductId = product.Id,
                        ProductInStock = amountOfProductsRemovedFromMagazine,
                        IsDeleted = false,
                        CreatedOn = DateTime.Now,
                        UpdatedBy = GetUser(context),
                        QunatityBeforeRemoval = (int)productToBeUpdated.ProductInStock,
                    };
                    _context.ProductRecordsChangeds.Add(copyOfRecord);
                    await _context.SaveChangesAsync();
                }
                return resultUpdate;
            }
            catch (Exception ex)
            {
                return ResponseService<ProductPostEditViewModel>.ExceptioThrow(ex.Message);
            }
        }

        // Delete a product
        public async Task<ResponseService<ProductViewModel>> DeleteProductAsync(Guid id)
        {
            try
            {
                var productToBeDeleted = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (productToBeDeleted is null)
                    return ResponseService<ProductViewModel>.NotFound($"Product with id {id} does not exists !!");
                _context.Products.Remove(productToBeDeleted);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ResponseService<ProductViewModel>.ExceptioThrow(ex.Message);
            }
            return ResponseService<ProductViewModel>.Deleted($"Product with id : {id} has been deleted!!!!");
        }

        // Display all products together with their category info
        public async Task<IEnumerable<ProductsAndCategoryInfoViewModel>> ProductsAndCategoryAsync()
        {
            var products = await _context.Products.Include(c => c.ProductCategory).ToListAsync();
            return _mapper.Map<List<ProductsAndCategoryInfoViewModel>>(products);
        }

        // Serach a product by its name
        public async Task<IEnumerable<ProductViewModel>> GetProductByNameAsync(string ProductName)
        {
            var product = await _context.Products.Include(c => c.ProductCategory).Where(p => p.ProductName.Contains(ProductName)).ToListAsync();
            return _mapper.Map<IEnumerable<ProductViewModel>>(product);
        }

        // Get product image
        public async Task<ResponseService<ProductImageOnly>> GetProductImage(Guid id)
        {
            try
            {
                var productImage = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (productImage == null)
                    return ResponseService<ProductImageOnly>.NotFound("Image does not exists");
                return ResponseService<ProductImageOnly>.Ok(_mapper.Map<ProductImageOnly>(productImage));
            }
            catch (Exception ex)
            {
                return ResponseService<ProductImageOnly>.ExceptioThrow(ex.Message);
            }
        }

        // Get changes made by employee
        public async Task<IEnumerable<ProductsRecordCopyViewModel>> GetProducChangesByEmpolyees()
        {
            var products = await _context.ProductRecordsChangeds.OrderByDescending(p => p.CreatedOn).ToListAsync();
            return _mapper.Map<List<ProductsRecordCopyViewModel>>(products);
        }

        // Delete the change made by employee
        public async Task<ResponseService<ProductsRecordCopyViewModel>> DeleteProductChangeByEmployee(Guid id)
        {
            try
            {
                var record = await _context.ProductRecordsChangeds.FirstOrDefaultAsync(p => p.Id == id);
                if (record is null)
                    return ResponseService<ProductsRecordCopyViewModel>.NotFound($"Record with id {id} does not exists !!");
                _context.ProductRecordsChangeds.Remove(record);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ResponseService<ProductsRecordCopyViewModel>.ExceptioThrow(ex.Message);
            }
            return ResponseService<ProductsRecordCopyViewModel>.Deleted($"Product with id : {id} has been deleted!!!!");
        }
    }
}