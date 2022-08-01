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
                ProductCategoryId = product.ProductCategoryId,
                CreatedBy = product.CreatedBy,
                CurrencyType = product.CurrencyType,
                SerialNumber = product.SerialNumber,
                ProductInStock = product.ProductInStock,
                CategoryName = product.ProductCategory.CategoryName
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
                Image = product.Image,
            };
        }

        public static ProductPostEditViewModel AsProductUpdateDto(this Product product)
        {
            return new ProductPostEditViewModel
            {
                Id = product.Id,
                ProductName = product.ProductName,
                SerialNumber = product.SerialNumber,
                Price = product.Price,
                ProductInStock = product.ProductInStock,
                CurrencyType = product.CurrencyType,
                ProductDescription = product.ProductDescription,
                Image = product.Image,
                CreatedBy = product.CreatedBy,
            };
        }

        public static ProductImageOnly AsProductImageDto(this Product product)
        {
            return new ProductImageOnly
            {
                Id = product.Id,
                Image = product.Image,
            };
        }

        public static ProductsRecordCopyViewModel AsProducChangesByEmpolyees(this ProductRecordsChanged product)
        {
            return new ProductsRecordCopyViewModel
            {
                ProductId = product.ProductId,
                ChangesInQunatity = product.ProductInStock,
                UpdatedBy = product.UpdatedBy,
                UpdatedOn = product.CreatedOn,
                Id = product.Id,
            };
        }
    }
}
