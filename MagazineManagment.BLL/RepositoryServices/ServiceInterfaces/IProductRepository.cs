using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DTO.ViewModels;

namespace MagazineManagment.BLL.RepositoryServices.ServiceInterfaces
{
    public interface IProductRepository
    {
        Task<ResponseService<ProductViewModel>> CreateProductAsync(ProductCreateViewModel product);
        Task<ResponseService<ProductViewModel>> DeleteProductAsync(Guid id);
        Task<IEnumerable<ProductViewModel>> GetAllProductsAsync();
        Task<ResponseService<ProductViewModel>> GetProductByNameAsync(string ProductName);
        Task<ResponseService<ProductsAndCategoryInfoViewModel>> GetProductAsync(Guid id);
        Task<IEnumerable<ProductsAndCategoryInfoViewModel>> ProductsAndCategoryAsync();
        Task<ResponseService<ProductViewModel>> UpdateProductAsync(ProductUpdateViewModel product);
    }
}