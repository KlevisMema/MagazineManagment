using MagazineManagment.DAL.Models;
using MagazineManagment.DTO.ViewModels;

namespace MagazineManagment.DTO.DataTransferObjects
{
    public static class CategoryDataTransferObj
    {
        public static CategoryViewModel AsCategoryDto(this Category category)
        {
            return new CategoryViewModel
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                CreatedOn = category.CreatedOn,
            };
        }

        public static CategoryNameOnlyViewModel AsCategoryNameOnlyDto(this Category category)
        {
            return new CategoryNameOnlyViewModel
            {
                CategoryName = category.CategoryName,
                Id = category.Id
            };
        }

    }
}
