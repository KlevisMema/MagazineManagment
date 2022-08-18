namespace MagazineManagment.DTO.ViewModels
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        public string? CategoryName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
    }
}