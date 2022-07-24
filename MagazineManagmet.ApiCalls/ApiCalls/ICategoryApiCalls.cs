using MagazineManagment.DTO.ViewModels;

namespace MagazineManagmet.ApiCalls.ApiCalls
{
    public interface ICategoryApiCalls
    {
        Task<IEnumerable<CategoryViewModel>> GetAllCategories();
        Task<HttpResponseMessage> CreateCategory(CategoryCreateViewModel category);
        Task<CategoryUpdateViewModel> EditCategory(Guid id);
    }
}