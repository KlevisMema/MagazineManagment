using System.ComponentModel.DataAnnotations;

namespace MagazineManagment.DTO.ViewModels
{
    public class CategoryCreateViewModel
    {
        [Required(ErrorMessage = "Category name is required")]
        [Display(Name = "Category name")]
        [StringLength(maximumLength:10,MinimumLength =2)]
        public string? CategoryName { get; set; }
        public string? CreatedBy { get; set; }
    }
}
