using System.ComponentModel.DataAnnotations;

namespace MagazineManagment.DTO.ViewModels
{
    public class CategoryCreateViewModel
    {
        //[Required(ErrorMessage = "Category name is required")]
        //[Display(Name = "Category name")]
        //[StringLength(maximumLength: 10, MinimumLength = 2)]
        //[RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please ensure that the category name doesn't contain special characters ex:(-/?><) etc.")]
        public string? CategoryName { get; set; }
        public string? CreatedBy { get; set; }
    }
}