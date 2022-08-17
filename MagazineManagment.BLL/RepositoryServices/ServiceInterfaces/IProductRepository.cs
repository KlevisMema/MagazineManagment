using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DTO.ViewModels;
using Microsoft.AspNetCore.Http;

namespace MagazineManagment.BLL.RepositoryServices.ServiceInterfaces
{
    public interface IProductRepository
    {
        Task<ResponseService<ProductViewModel>> CreateProductAsync(ProductCreateViewModelNoIFormFile product, HttpContext context);
        Task<ResponseService<ProductViewModel>> DeleteProductAsync(Guid id);
        Task<IEnumerable<ProductViewModel>> GetAllProductsAsync();
        Task<IEnumerable<ProductViewModel>> GetProductByNameAsync(string ProductName);
        Task<ResponseService<ProductViewModel>> GetProductAsync(Guid id);
        Task<IEnumerable<ProductsAndCategoryInfoViewModel>> ProductsAndCategoryAsync();
        Task<ResponseService<ProductPostEditViewModel>> UpdateProductAsync(ProductPostEditViewModel product, HttpContext context);
        Task<ResponseService<ProductImageOnly>> GetProductImage(Guid id);
        Task<IEnumerable<ProductsRecordCopyViewModel>> GetProducChangesByEmpolyees();
        Task<ResponseService<ProductsRecordCopyViewModel>> DeleteProductChangeByEmployee(Guid id);
    }
}