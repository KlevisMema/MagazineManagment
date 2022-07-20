using MagazineManagment.Shared.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MagazineManagment.DTO.ViewModels
{
    public class ProductUpdateViewModel
    {
        //[Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [Display(Name = "Product name")]
        [StringLength(maximumLength: 10, MinimumLength = 2)]
        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Serial number is required")]
        [Display(Name = "Serial number")]
        public string SerialNumber { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Product in stock field is required")]
        [Display(Name = "Product in stock")]
        public int ProductInStock { get; set; }

        [Required(ErrorMessage = "Currency type is required")]
        [Display(Name = "Currency type")]
        public CurrencyTypeEnum CurrencyType { get; set; }

        [Display(Name = "Product description")]
        [StringLength(maximumLength: 30, MinimumLength = 4)]
        public string? ProductDescription { get; set; }

        //public string? Image { get; set; }

        //[Display(Name = "Image")]
        //public IFormFile ImageFile { get; set; }

        public string? CreatedBy { get; set; }
    }
}
