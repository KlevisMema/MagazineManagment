using MagazineManagment.DTO.ViewModels;

namespace MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces
{
    public interface IProductApiCalls
    {
        Task<IEnumerable<ProductViewModel>> GetAllProducts(string token);
        Task<IEnumerable<CategoryNameOnlyViewModel>> GetCreateProduct(string token);
        Task<HttpResponseMessage> PostCreateProduct(ProductCreateViewModel product, string token);
        Task<ProductUpdateViewModel> GetEditProduct(Guid id, string token);
        Task<HttpResponseMessage> PostEditProduct(ProductUpdateViewModel UpdateProduct, string token);
        Task<HttpResponseMessage> Delete(Guid id, string token);
        Task<IEnumerable<ProductsRecordCopyViewModel>> GetProductChangesByEmpolyees(string token);
        Task<HttpResponseMessage> DeleteProductChangeByEmployee(Guid id, string token);
        Task<ProductViewModel> DetailsOfProductChangedByEmployee(Guid id, string token);
        Task<IEnumerable<ProductViewModel>> SearchProduct(string productName, string token);
    }
}