
using System.ComponentModel.DataAnnotations;

namespace MagazineManagment.DTO.ViewModels
{
    public class RolesGetAllDetails
    {
        public string RoleId { get; set; }

        [Display(Name = "Role name")]
        public  string RoleName { get; set; }

        [Display(Name = "Role name normalized")]
        public string RoleNameNormalized { get; set; }
    }
}
