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
                Image = product.Image,
                ProductCategoryId = product.ProductCategoryId
            };
        }

        public static ProductsAndCategoryInfoViewModel AsProductCategoryDto(this Product product)
        {
            return new ProductsAndCategoryInfoViewModel
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CreatedOn = product.CreatedOn,
                ProductDescription = product.ProductDescription,
                Price = product.Price,
                Image = product.Image,
                CategoryId = product.ProductCategoryId,
                CategoryName = product.ProductCategory.CategoryName
            };
        }
    }
}
