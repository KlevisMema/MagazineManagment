using MagazineManagment.DTO.ViewModels;

namespace MagazineManagment.Web.ApiCalls
{
    public interface IProductApiCalls
    {
        Task<IEnumerable<ProductViewModel>> GetAllProducts();
        Task<IEnumerable<CategoryNameOnlyViewModel>> GetCreateProduct();
        Task<HttpResponseMessage> PostCreateProduct(ProductCreateViewModel product);
        Task<ProductUpdateViewModel> GetEditProduct(Guid id);
        Task<HttpResponseMessage> PostEditProduct(ProductUpdateViewModel UpdateProduct);
        Task<HttpResponseMessage> Delete(Guid id);
    }
}