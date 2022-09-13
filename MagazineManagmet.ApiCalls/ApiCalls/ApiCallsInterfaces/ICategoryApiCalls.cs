using MagazineManagment.DTO.ViewModels;

namespace MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces
{
    public interface ICategoryApiCalls
    {
        Task<IEnumerable<CategoryViewModel>> GetAllCategories(string token);
        Task<HttpResponseMessage> PostCreateCategory(CategoryCreateViewModel category, string token);
        Task<CategoryViewModel> GetEditCategory(Guid id, string token);
        Task<HttpResponseMessage> PostEditCategory(CategoryUpdateViewModel category, string token);
        Task<HttpResponseMessage> PostDeleteCategory(Guid id, string token);
        Task<CategoryViewModel> ActivateCategory(Guid id, string token);
    }
}