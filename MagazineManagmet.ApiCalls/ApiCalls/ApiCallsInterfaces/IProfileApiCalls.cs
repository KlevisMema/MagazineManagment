using MagazineManagment.DTO.ViewModels;

namespace MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces
{
    public interface IProfileApiCalls
    {
        Task<IEnumerable<RolesGetAllDetails>> GetAllRoles();
        Task<HttpResponseMessage> PostCreateRole(RoleCreateViewModel role);
        Task<RoleFindViewModel> GetEditRole(string id);
        Task<HttpResponseMessage> PostUpdateRole(ProfileUpdateViewModel role);
        Task<RolesGetAllDetails> GetAllRolesDetails(string id);
        Task<HttpResponseMessage> DeleteRole(string id);
        Task<IEnumerable<UserInRoleViewModel>> UsersInRole(string id);
        Task<IEnumerable<UserInRoleViewModel>> GetAllUsers(string id);
        Task<HttpResponseMessage> AssignRoleToUsers(List<UserInRoleViewModel> users, string id);
    }
}