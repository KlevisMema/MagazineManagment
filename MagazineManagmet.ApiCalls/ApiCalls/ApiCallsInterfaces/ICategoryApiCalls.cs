using MagazineManagment.DTO.ViewModels;

namespace MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces
{
    public interface ICategoryApiCalls
    {
        Task<IEnumerable<CategoryViewModel>> GetAllCategories();
        Task<HttpResponseMessage> PostCreateCategory(CategoryCreateViewModel category);
        Task<CategoryUpdateViewModel> GetEditCategory(Guid id);
        Task<HttpResponseMessage> PostEditCategory(CategoryUpdateViewModel category);
        Task<HttpResponseMessage> PostDeleteCategory(Guid id);
    }
}