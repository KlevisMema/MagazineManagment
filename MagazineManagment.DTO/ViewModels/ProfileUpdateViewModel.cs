using System.ComponentModel.DataAnnotations;

namespace MagazineManagment.DTO.ViewModels
{
    public class ProfileUpdateViewModel
    {
        public string RoleId { get; set; }
        [Required(ErrorMessage = "Role name field is required")]
        [Display(Name = "Role name")]
        [StringLength(maximumLength: 10, MinimumLength = 4)]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please ensure that the role name doesn't contain special characters ex:(-/?><) etc.")]
        public string RoleName { get; set; } = string.Empty;
    }
}