using MagazineManagment.Shared.Enums;

namespace MagazineManagment.DTO.ViewModels
{
    public class ProductPostEditViewModel
    {
        public Guid Id { get; set; }
        public string? ProductName { get; set; }
        public string? SerialNumber { get; set; }
        public decimal? Price { get; set; }
        public int? ProductInStock { get; set; }
        public CurrencyTypeEnum? CurrencyType { get; set; }
        public string? ProductDescription { get; set; }
        public string? Image { get; set; }
        public string? CreatedBy { get; set; }
        public string? UserName { get; set; }
    }
}
