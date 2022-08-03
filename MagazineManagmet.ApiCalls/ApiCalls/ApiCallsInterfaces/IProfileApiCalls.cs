using MagazineManagment.DTO.ViewModels;

namespace MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces
{
    public interface IProfileApiCalls
    {
        Task<IEnumerable<RolesGetAllDetails>> GetAllRoles();
        Task<HttpResponseMessage> PostCreateRole(RoleCreateViewModel role);
        Task<ProfileUpdateViewModel> GetEditRole(string id);
        Task<HttpResponseMessage> PostUpdateRole(ProfileUpdateViewModel role);
        Task<RolesGetAllDetails> GetAllRolesDetails(string id);
        Task<HttpResponseMessage> DeleteRole(string id);
        Task<IEnumerable<UserInRoleViewModel>> GetAllUsersInRole(string id);
        Task<IEnumerable<UserNotInRoleViewModel>> GetAllUsersNotInRole(string id);
        Task<HttpResponseMessage> AssignRoleToUsers(List<UserInRoleViewModel> users, string id);
        Task<HttpResponseMessage> RemoveRoleFromUsers(List<UserInRoleViewModel> users, string id);
    }
}