using System.ComponentModel.DataAnnotations;

namespace MagazineManagment.DTO.ViewModels
{
    public class CategoryUpdateViewModel
    {
        [Required(ErrorMessage = "Category id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(maximumLength: 10, MinimumLength = 2)]
        public string? CategoryName { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
