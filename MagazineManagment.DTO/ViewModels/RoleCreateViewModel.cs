using System.ComponentModel.DataAnnotations;

namespace MagazineManagment.DTO.ViewModels
{
    public class RoleCreateViewModel
    {
        [Required]
        public string RoleName { get; set; } = string.Empty;
    }
}
