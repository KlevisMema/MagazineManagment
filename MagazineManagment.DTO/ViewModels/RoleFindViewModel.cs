using System.ComponentModel.DataAnnotations;

namespace MagazineManagment.DTO.ViewModels
{
    public class RoleFindViewModel
    {
        public string RoleId { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
