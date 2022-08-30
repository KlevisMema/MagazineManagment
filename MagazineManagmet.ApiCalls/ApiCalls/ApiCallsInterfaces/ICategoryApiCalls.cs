using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DTO.ViewModels;

namespace MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces
{
    public interface ICategoryApiCalls
    {
        Task<IEnumerable<CategoryViewModel>> GetAllCategories();
        Task<HttpResponseMessage> PostCreateCategory(CategoryCreateViewModel category);
        Task<CategoryViewModel> GetEditCategory(Guid id);
        Task<HttpResponseMessage> PostEditCategory(CategoryUpdateViewModel category);
        Task<HttpResponseMessage> PostDeleteCategory(Guid id);
        Task<CategoryViewModel> ActivateCategory(Guid id);
    }
}