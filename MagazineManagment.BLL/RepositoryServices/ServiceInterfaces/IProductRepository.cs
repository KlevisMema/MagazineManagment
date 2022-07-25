using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DTO.ViewModels;

namespace MagazineManagment.BLL.RepositoryServices.ServiceInterfaces
{
    public interface IProductRepository
    {
        Task<ResponseService<ProductViewModel>> CreateProductAsync(ProductCreateViewModelNoIFormFile product);
        Task<ResponseService<ProductViewModel>> DeleteProductAsync(Guid id);
        Task<IEnumerable<ProductViewModel>> GetAllProductsAsync();
        Task<ResponseService<ProductViewModel>> GetProductByNameAsync(string ProductName);
        Task<ResponseService<ProductViewModel>> GetProductAsync(Guid id);
        Task<IEnumerable<ProductsAndCategoryInfoViewModel>> ProductsAndCategoryAsync();
        Task<ResponseService<ProductPostEditViewModel>> UpdateProductAsync(ProductPostEditViewModel product);
        Task<ResponseService<ProductImageOnly>> GetProductImage(Guid id);
    }
}