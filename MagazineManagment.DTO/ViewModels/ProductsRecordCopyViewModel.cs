namespace MagazineManagment.DTO.ViewModels
{
    public class ProductsRecordCopyViewModel
    {
        public Guid ProductId { get; set; }
        public int? ChangesInQunatity { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
