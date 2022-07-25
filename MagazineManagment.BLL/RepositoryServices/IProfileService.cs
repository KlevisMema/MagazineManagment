using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DTO.ViewModels;

namespace MagazineManagment.BLL.RepositoryServices
{
    public interface IProfileService
    {
        IEnumerable<RoleCreateViewModel> GetRoles();
        Task<ResponseService<RoleCreateViewModel>> CreateRole(RoleCreateViewModel roleName);
        Task<ResponseService<RoleFindViewModel>> FindRole(string roleId);
        Task<ResponseService<RoleFindViewModel>> UpdateRole(RoleFindViewModel updateRole);
        Task<ResponseService<RoleFindViewModel>> DeleteRole(string id);
    }
}