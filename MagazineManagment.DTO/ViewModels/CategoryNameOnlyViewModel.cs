using System.ComponentModel.DataAnnotations;

namespace MagazineManagment.DTO.ViewModels
{
    public class CategoryNameOnlyViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Category is required")]
        public string? CategoryName { get; set; }
    }
}
