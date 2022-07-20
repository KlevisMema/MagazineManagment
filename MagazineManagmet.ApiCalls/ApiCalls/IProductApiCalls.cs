using MagazineManagment.DTO.ViewModels;

namespace MagazineManagment.Web.ApiCalls
{
    public interface IProductApiCalls
    {
        IEnumerable<ProductViewModel> GetAllProducts();
        IEnumerable<CategoryNameOnlyViewModel> GetCreateProduct();
        //Task<ProductCreateViewModel> PostCreateProduct(ProductCreateViewModel product);
    }
}