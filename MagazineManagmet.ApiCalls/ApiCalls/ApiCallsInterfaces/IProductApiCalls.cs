using MagazineManagment.DTO.ViewModels;
using Microsoft.AspNetCore.Http;

namespace MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces
{
    public interface IProductApiCalls
    {
        Task<IEnumerable<ProductViewModel>> GetAllProducts();
        Task<IEnumerable<CategoryNameOnlyViewModel>> GetCreateProduct();
        Task<HttpResponseMessage> PostCreateProduct(ProductCreateViewModel product);
        Task<ProductUpdateViewModel> GetEditProduct(Guid id);
        Task<HttpResponseMessage> PostEditProduct(ProductUpdateViewModel UpdateProduct);
        Task<HttpResponseMessage> Delete(Guid id);
        Task<IEnumerable<ProductsRecordCopyViewModel>> GetProductChangesByEmpolyees();
        Task<HttpResponseMessage> DeleteProductChangeByEmployee(Guid id);
        Task<ProductViewModel> DetailsOfProductChangedByEmployee(Guid id);
    }
}