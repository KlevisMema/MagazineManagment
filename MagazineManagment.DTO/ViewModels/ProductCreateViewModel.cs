using MagazineManagment.Shared.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MagazineManagment.DTO.ViewModels
{
    public class ProductCreateViewModel
    {
        [Required(ErrorMessage = "Product name is required")]
        [Display(Name = "Product name")]
        [StringLength(maximumLength: 20, MinimumLength = 2)]
        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Serial number is required")]
        [Display(Name = "Serial number")]
        [StringLength(maximumLength: 5, MinimumLength = 5)]
        public string? SerialNumber { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [DataType(DataType.Currency)]
        [Range(minimum: 20, maximum: 10000)]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Product in stock field is required")]
        [Display(Name = "Product in stock")]
        [Range(minimum: 1, maximum: 100)]
        public  int? ProductInStock { get; set; }

        [Required(ErrorMessage = "Currency type is required")]
        [Display(Name = "Currency type")]
        public CurrencyTypeEnum? CurrencyType { get; set; }

        [Display(Name = "Product description")]
        [StringLength(maximumLength: 30, MinimumLength = 4)]
        public string? ProductDescription { get; set; }

        public string? Image { get; set; }

        [Required(ErrorMessage = "Product imageis required")]
        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public Guid? ProductCategoryId { get; set; }
        public string? CreatedBy { get; set; }
    }
}
