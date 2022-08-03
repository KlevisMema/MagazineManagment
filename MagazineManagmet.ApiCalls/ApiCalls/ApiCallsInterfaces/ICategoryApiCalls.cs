using MagazineManagment.DTO.ViewModels;
using Microsoft.AspNetCore.Http;

namespace MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces
{
    public interface ICategoryApiCalls
    {
        Task<IEnumerable<CategoryViewModel>> GetAllCategories();
        Task<HttpResponseMessage> PostCreateCategory(CategoryCreateViewModel category, HttpContext context);
        Task<CategoryUpdateViewModel> GetEditCategory(Guid id);
        Task<HttpResponseMessage> PostEditCategory(CategoryUpdateViewModel category, HttpContext context);
        Task<HttpResponseMessage> PostDeleteCategory(Guid id);
    }
}