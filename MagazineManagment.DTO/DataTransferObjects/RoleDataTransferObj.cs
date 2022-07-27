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

        public static RolesGetAllDetails AsRoleGetAllDetailsDto(this IdentityRole role)
        {
            return new RolesGetAllDetails
            {
                RoleNameNormalized = role.NormalizedName,
                RoleName = role.Name,
                RoleId = role.Id,
            };
        }

        public static ProfileUpdateViewModel AsRoleUpdateDto(this IdentityRole role)
        {
            return new ProfileUpdateViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
            };
        }


        public static UserInRoleViewModel AsUserInRoleDto(this IdentityUser userInRole)
        {
            return new UserInRoleViewModel
            {
               UserId = userInRole.Id,
               Username = userInRole.UserName,
               IsSelected = true
            };
        }

        public static UserInRoleViewModel AsUserNotInRoleDto(this IdentityUser userInRole)
        {
            return new UserInRoleViewModel
            {
                UserId = userInRole.Id,
                Username = userInRole.UserName,
                IsSelected = false
            };
        }

        public static UserInRoleViewModel AsUsersDto(this IdentityUser user)
        {
            return new UserInRoleViewModel
            {
                UserId = user.Id,
                Username = user.UserName,
            };
        }

    }
}
