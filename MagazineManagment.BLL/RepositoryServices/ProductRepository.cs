using MagazineManagment.BLL.RepositoryServices.ServiceInterfaces;
using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DAL.DataContext;
using MagazineManagment.DAL.Models;
using MagazineManagment.DTO.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MagazineManagment.Shared.UsersSeedValues;
using AutoMapper;

namespace MagazineManagment.BLL.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Microsoft.AspNetCore.Identity.IdentityUser> _user;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext context, UserManager<Microsoft.AspNetCore.Identity.IdentityUser> user, IMapper mapper)
        {
            _context = context;
            _user = user;
            _mapper = mapper;
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
        public async Task<ResponseService<ProductViewModel>> CreateProductAsync(ProductCreateViewModelNoIFormFile product)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == product.ProductCategoryId);

                if (category is null)
                    return ResponseService<ProductViewModel>.NotFound("Category not found");

                var checkIfSerialNrExists = await _context.Products.AnyAsync(sNr => sNr.SerialNumber == product.SerialNumber.ToUpper());

                if (checkIfSerialNrExists)
                    return ResponseService<ProductViewModel>.ErrorMsg($"Serial number {product.SerialNumber} exists, please give another  serial number");
                var newProduct = _mapper.Map<Product>(product);

                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();
                return ResponseService<ProductViewModel>.Ok(_mapper.Map<ProductViewModel>(newProduct));
            }
            catch (Exception ex)
            {
                return ResponseService<ProductViewModel>.ExceptioThrow(ex.Message);
            }
        }

        // Update a Product 
        public async Task<ResponseService<ProductPostEditViewModel>> UpdateProductAsync(ProductPostEditViewModel product)
        {
            var productToBeUpdated = await _context.Products.FirstOrDefaultAsync(c => c.Id == product.Id);
            if (productToBeUpdated == null)
                return ResponseService<ProductPostEditViewModel>.NotFound("Product does not exists");


            // find  the user by email
            var findUser = await _user.FindByEmailAsync(product.UserName);
            if (findUser == null)
                return ResponseService<ProductPostEditViewModel>.NotFound("User not found");


            // find the role of the user
            var role = await _user.GetRolesAsync(findUser);
            if (role == null)
                return ResponseService<ProductPostEditViewModel>.NotFound("Role not found");


            // if empolyee role then check how many products is he removing
            bool recordChangedByEmployee = false;
            if (product.ProductInStock > productToBeUpdated.ProductInStock && role.Contains(RoleName.Employee))
                return ResponseService<ProductPostEditViewModel>.ErrorMsg("You can't insert products in quantity");

            var amountOfProductsRemovedFromMagazine = product.ProductInStock - productToBeUpdated.ProductInStock;
            if (role.Contains(RoleName.Employee) && amountOfProductsRemovedFromMagazine <= -20)
                return ResponseService<ProductPostEditViewModel>.ErrorMsg("You can not remove more than 20 products ");
            else if (role.Contains(RoleName.Employee) && amountOfProductsRemovedFromMagazine == 0)
                recordChangedByEmployee = false;
            else if (role.Contains(RoleName.Employee) && amountOfProductsRemovedFromMagazine > -20)
                recordChangedByEmployee = true;



            //  check if the serial number is changed, if yes  then check if does exists 
            bool ckeckIfExists = false;
            if (productToBeUpdated.SerialNumber != product.SerialNumber)
                ckeckIfExists = await _context.Products.AnyAsync(s => s.SerialNumber == product.SerialNumber);
            if (ckeckIfExists)
                return ResponseService<ProductPostEditViewModel>.ErrorMsg($"Serial number {product.SerialNumber} exists, please give another  serial number");

            try
            {
                if (recordChangedByEmployee)
                {
                    ProductRecordsChanged copyOfRecord = new()
                    {
                        ProductId = product.Id,
                        ProductInStock = amountOfProductsRemovedFromMagazine,
                        IsDeleted = false,
                        CreatedBy = product.CreatedBy,
                        CreatedOn = DateTime.Now,
                        UpdatedBy = product.UserName,
                        QunatityBeforeRemoval = (int)productToBeUpdated.ProductInStock
                    };
                    _context.ProductRecordsChangeds.Add(copyOfRecord);
                    await _context.SaveChangesAsync();

                    productToBeUpdated.ProductInStock = product.ProductInStock;
                }
                else
                {
                   _mapper.Map(product,productToBeUpdated);
                }
                _context.Products.Update(productToBeUpdated);
                await _context.SaveChangesAsync();

                return ResponseService<ProductPostEditViewModel>.Ok(_mapper.Map<ProductPostEditViewModel>(productToBeUpdated));
            }
            catch (Exception ex)
            {
                return ResponseService<ProductPostEditViewModel>.ExceptioThrow(ex.Message);
            }
        }

        //Delete a product
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
        public async Task<ResponseService<ProductViewModel>> GetProductByNameAsync(string ProductName)
        {
            try
            {
                var product = await _context.Products.Include(c => c.ProductCategory).FirstOrDefaultAsync(p => p.ProductName == ProductName);
                if (product == null)
                    return ResponseService<ProductViewModel>.NotFound("Product does not exists");
                return ResponseService<ProductViewModel>.Ok(_mapper.Map<ProductViewModel>(product));
            }
            catch (Exception ex)
            {
                return ResponseService<ProductViewModel>.ExceptioThrow(ex.Message);
            }
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

        // get changes made by employee
        public async Task<IEnumerable<ProductsRecordCopyViewModel>> GetProducChangesByEmpolyees()
        {
            var products = await _context.ProductRecordsChangeds.OrderByDescending(p => p.CreatedOn).ToListAsync();
            return _mapper.Map<List<ProductsRecordCopyViewModel>>(products);
        }

        // delete the change made by employee
        public async Task<ResponseService<ProductsRecordCopyViewModel>> DeleteProductChangeByEmployee(Guid id)
        {
            try
            {
                var record = await _context.ProductRecordsChangeds.FirstOrDefaultAsync(p => p.Id == id);
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