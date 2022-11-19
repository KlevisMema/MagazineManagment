namespace MagazineManagment.DTO.ViewModels
{
    public class FullUserInfoViewModel
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public IList<string>? Role { get; set; }
    }
}