using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MagazineManagment.DTO.ViewModels
{
    public class ProductCreateViewModel
    {
        [Required(ErrorMessage = "Product name is required")]
        [Display(Name = "Product name")]
        [StringLength(maximumLength: 10, MinimumLength = 2)]
        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Product description")]
        [StringLength(maximumLength: 30, MinimumLength = 4)]
        public string? ProductDescription { get; set; }

        public string? Image { get; set; }

        public IFormFile ImageFile { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public Guid? ProductCategoryId { get; set; }
    }
}
