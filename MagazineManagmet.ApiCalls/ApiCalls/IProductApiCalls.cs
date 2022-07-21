﻿using MagazineManagment.DTO.ViewModels;

namespace MagazineManagment.Web.ApiCalls
{
    public interface IProductApiCalls
    {
        IEnumerable<ProductViewModel> GetAllProducts();
        IEnumerable<CategoryNameOnlyViewModel> GetCreateProduct();
        Task<HttpResponseMessage> PostCreateProduct(ProductCreateViewModel product);
        ProductUpdateViewModel GetEdit(Guid id);
        void PostEdit(ProductUpdateViewModel UpdateProduct);
        void Delete(Guid id);
    }
}