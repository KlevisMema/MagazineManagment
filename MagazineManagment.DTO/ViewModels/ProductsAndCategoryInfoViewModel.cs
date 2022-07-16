using System.ComponentModel.DataAnnotations;

namespace MagazineManagment.DTO.ViewModels
{
    public class ProductsAndCategoryInfoViewModel
    {
        public Guid Id { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public string? ProductDescription { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? Image { get; set; }
        public Guid? CategoryId { get; set; }
        [Display(Name = "Category")]
        public string? CategoryName { get; set; }
        public DateTime? CategoryCreatedDate { get; set; }
    }
}
