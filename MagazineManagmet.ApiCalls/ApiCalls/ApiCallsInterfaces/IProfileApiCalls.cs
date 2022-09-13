using MagazineManagment.DTO.ViewModels;

namespace MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces
{
    public interface IProfileApiCalls
    {
        Task<IEnumerable<RolesGetAllDetails>> GetAllRoles(string token);
        Task<HttpResponseMessage> PostCreateRole(RoleCreateViewModel role, string token);
        Task<ProfileUpdateViewModel> GetEditRole(string id, string token);
        Task<HttpResponseMessage> PostUpdateRole(ProfileUpdateViewModel role, string token);
        Task<RolesGetAllDetails> GetAllRolesDetails(string id, string token);
        Task<HttpResponseMessage> DeleteRole(string id, string token);
        Task<IEnumerable<UserInRoleViewModel>> GetAllUsersInRole(string id, string token);
        Task<IEnumerable<UserNotInRoleViewModel>> GetAllUsersNotInRole(string id, string token);
        Task<HttpResponseMessage> AssignRoleToUsers(List<UserInRoleViewModel> users, string id, string token);
        Task<HttpResponseMessage> RemoveRoleFromUsers(List<UserInRoleViewModel> users, string id, string token);
    }
}