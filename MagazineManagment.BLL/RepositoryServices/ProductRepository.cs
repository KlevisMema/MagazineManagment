﻿using MagazineManagment.BLL.RepositoryServices.ServiceInterfaces;
using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DAL.DataContext;
using MagazineManagment.DTO.DataTransferObjects;
using MagazineManagment.DAL.Models;
using MagazineManagment.DTO.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace MagazineManagment.BLL.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Microsoft.AspNetCore.Identity.IdentityUser> _user;

        public ProductRepository(ApplicationDbContext context, UserManager<Microsoft.AspNetCore.Identity.IdentityUser> user)
        {
            _context = context;
            _user = user;
        }

        //Get all products
        public async Task<IEnumerable<ProductViewModel>> GetAllProductsAsync()
        {
            var products = await _context.Products.Include(p => p.ProductCategory).Select(x => x.AsProductDto()).ToListAsync();
            return products;
        }

        //Get a single product by id
        public async Task<ResponseService<ProductViewModel>> GetProductAsync(Guid id)
        {
            var product = await _context.Products.Include(p => p.ProductCategory).FirstOrDefaultAsync(x => x.Id == id);

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


            var checkIfSerialNrExists = await _context.Products.AnyAsync(sNr => sNr.SerialNumber == product.SerialNumber.ToUpper());
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
            var amountOfProductsRemovedFromMagazine = productToBeUpdated.ProductInStock - product.ProductInStock;
            if (role.Contains("Employee") && amountOfProductsRemovedFromMagazine >= 20)
                return ResponseService<ProductPostEditViewModel>.ErrorMsg("You can not remove more than 20 products ");
            else if (role.Contains("Employee") && amountOfProductsRemovedFromMagazine == 0)
                recordChangedByEmployee = false;
            else
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
                        UpdatedBy = product.UserName
                    };
                    _context.ProductRecordsChangeds.Add(copyOfRecord);
                    await _context.SaveChangesAsync();

                    productToBeUpdated.ProductInStock = product.ProductInStock;
                }
                else
                {
                    productToBeUpdated.ProductName = product.ProductName;
                    productToBeUpdated.Price = product.Price;
                    productToBeUpdated.CreatedOn = DateTime.Now;
                    productToBeUpdated.ProductDescription = product.ProductDescription;
                    productToBeUpdated.SerialNumber = product.SerialNumber.ToUpper();
                    productToBeUpdated.CreatedBy = "Admin";
                    productToBeUpdated.Image = product.Image;
                    productToBeUpdated.ProductInStock = product.ProductInStock;
                }
                _context.Products.Update(productToBeUpdated);
                await _context.SaveChangesAsync();

                return ResponseService<ProductPostEditViewModel>.Ok(productToBeUpdated.AsProductUpdateDto());
            }
            catch (Exception ex)
            {
                return ResponseService<ProductPostEditViewModel>.ExceptioThrow(ex.Message);
            }
        }

        //Delete a product
        public async Task<ResponseService<ProductViewModel>> DeleteProductAsync(Guid id)
        {
            var productToBeDeleted = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (productToBeDeleted is null)
            {
                return ResponseService<ProductViewModel>.NotFound($"Product with id {id} does not exists !!");
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

            return ResponseService<ProductViewModel>.Deleted($"Product with id : {id} has been deleted!!!!");
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
                return ResponseService<ProductViewModel>.NotFound("Product does not exists");
            return ResponseService<ProductViewModel>.Ok(product.AsProductDto());
        }

        // Get product image
        public async Task<ResponseService<ProductImageOnly>> GetProductImage(Guid id)
        {

            var productImage = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (productImage == null)
                return ResponseService<ProductImageOnly>.NotFound("Image does not exists");

            return ResponseService<ProductImageOnly>.Ok(productImage.AsProductImageDto());
        }

        // get changes made by employee
        public async Task<IEnumerable<ProductsRecordCopyViewModel>> GetProducChangesByEmpolyees()
        {
            var products = await _context.ProductRecordsChangeds.ToListAsync();
            return products.Select(p => p.AsProducChangesByEmpolyees());
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