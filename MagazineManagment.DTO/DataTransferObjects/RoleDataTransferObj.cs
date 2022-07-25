using MagazineManagment.DTO.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace MagazineManagment.DTO.DataTransferObjects
{
    public static class RoleDataTransferObj
    {
        public static RoleCreateViewModel AsRoleCreateDto(this IdentityRole role)
        {
            return new RoleCreateViewModel
            {
                RoleName = role.Name,
            };
        }

        public static RoleFindViewModel AsRoleEditDto(this IdentityRole role)
        {
            return new RoleFindViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
            };
        }
    }
}
