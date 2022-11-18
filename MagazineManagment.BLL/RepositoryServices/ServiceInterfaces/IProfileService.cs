using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DTO.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace MagazineManagment.BLL.RepositoryServices.ServiceInterfaces
{
    public interface IProfileService
    {
        Task<ResponseService<Tuple<string?, IdentityUser?>>> Login(LoginViewModel inputModel, string key);
        Task<ResponseService<string>> Register(RegisterViewModel Input, string key);
        Task<IEnumerable<RolesGetAllDetails>> GetRoles();
        Task<ResponseService<RoleCreateViewModel>> CreateRole(RoleCreateViewModel roleName);
        Task<ResponseService<RoleFindViewModel>> FindRole(string roleId);
        Task<ResponseService<ProfileUpdateViewModel>> UpdateRole(ProfileUpdateViewModel updateRole);
        Task<ResponseService<RoleFindViewModel>> DeleteRole(string id);
        Task<ResponseService<RolesGetAllDetails>> GetRolesDetails(string id);
        Task<ResponseService<IEnumerable<UserInRoleViewModel>>> GetUsersOfARole(string roleId);
        Task<ResponseService<IEnumerable<UserNotInRoleViewModel>>> GettAllUsers(string id);
        Task<ResponseService<IEnumerable<UserInRoleViewModel>>> AssignRoleToUsers(List<UserInRoleViewModel> users, string id);
        Task<ResponseService<IEnumerable<UserInRoleViewModel>>> RemoveUsersFromRole(List<UserInRoleViewModel> users, string id);
    }
}