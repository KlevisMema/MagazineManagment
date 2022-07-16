using System.ComponentModel.DataAnnotations;

namespace MagazineManagment.DTO.ViewModels
{
    public class ProductUpdateViewModel
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

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

        public Guid? CategoryId { get; set; }

    }
}
