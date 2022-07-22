using MagazineManagment.DTO.ViewModels;

namespace MagazineManagment.Web.ApiCalls
{
    public interface IProductApiCalls
    {
        IEnumerable<ProductViewModel> GetAllProducts();
        IEnumerable<CategoryNameOnlyViewModel> GetCreateProduct();
        Task<HttpResponseMessage> PostCreateProduct(ProductCreateViewModel product);
        Task<ProductUpdateViewModel> GetEditProduct(Guid id);
        Task<HttpResponseMessage> PostEditProduct(ProductUpdateViewModel UpdateProduct);
        void Delete(Guid id);
    }
}