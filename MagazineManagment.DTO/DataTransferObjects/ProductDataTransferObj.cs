using MagazineManagment.DAL.Models;
using MagazineManagment.DTO.ViewModels;

namespace MagazineManagment.DTO.DataTransferObjects
{
    public static class ProductDataTransferObj
    {
        public static ProductViewModel AsProductDto(this Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CreatedOn = product.CreatedOn,
                ProductDescription = product.ProductDescription,
                Price = product.Price,
                //Image = product.Image,
                //ProductCategoryId = product.ProductCategoryId,
                CreatedBy = product.CreatedBy,
                CurrencyType = product.CurrencyType,
                SerialNumber = product.SerialNumber,
                ProductInStock = product.ProductInStock,
            };
        }

        public static ProductsAndCategoryInfoViewModel AsProductCategoryDto(this Product product)
        {
            return new ProductsAndCategoryInfoViewModel
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                CurrencyType = product.CurrencyType,
                CreatedOn = product.CreatedOn,
                CreatedBy = product.CreatedBy,
                ProductDescription = product.ProductDescription,
                CategoryId = product.ProductCategoryId,
                CategoryName = product.ProductCategory.CategoryName,
                //Image = product.Image,
            };
        }
    }
}
