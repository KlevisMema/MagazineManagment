using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DTO.ViewModels;
using Microsoft.AspNetCore.Http;

namespace MagazineManagment.BLL.RepositoryServices.ServiceInterfaces
{
    public interface ICategoryRepository
    {
        Task<ResponseService<CategoryViewModel>> CreateCategoryAsync(CategoryCreateViewModel category, HttpContext context);
        Task<ResponseService<CategoryViewModel>> DeleteCategoryAsync(Guid id);
        Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync();
        Task<ResponseService<CategoryViewModel>> GetCategoryAsync(Guid id);
        Task<IEnumerable<CategoryNameOnlyViewModel>> GetNamesOnlyCategories();
        Task<ResponseService<CategoryViewModel>> UpdateCategoryAsync(CategoryUpdateViewModel category, HttpContext context);
        Task<ResponseService<CategoryViewModel>> ActivateCategory(Guid id);
    }
}