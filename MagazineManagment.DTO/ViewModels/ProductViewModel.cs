using Microsoft.AspNetCore.Http;


namespace MagazineManagment.DTO.ViewModels
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public string? ProductDescription { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? Image { get; set; }
        public IFormFile ImageFile { get; set; }
        public Guid? ProductCategoryId { get; set; }
    }
}
