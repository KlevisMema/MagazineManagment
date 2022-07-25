using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DTO.DataTransferObjects;
using MagazineManagment.DTO.ViewModels;
using Microsoft.AspNetCore.Identity;


namespace MagazineManagment.BLL.RepositoryServices
{
    public class ProfileService : IProfileService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public ProfileService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }


        public IEnumerable<RoleCreateViewModel> GetRoles()
        {
            var getRoles = _roleManager.Roles.ToList();
            return getRoles.Select(roles => roles.AsRoleCreateDto());
        }

        public async Task<ResponseService<RoleCreateViewModel>> CreateRole(RoleCreateViewModel roleName)
        {
            try
            {
                IdentityRole identityRole = new()
                {
                    Name = roleName.RoleName,
                };
                await _roleManager.CreateAsync(identityRole);

                return ResponseService<RoleCreateViewModel>.Ok(identityRole.AsRoleCreateDto());
            }
            catch (Exception ex)
            {

                return ResponseService<RoleCreateViewModel>.ExceptioThrow(ex.Message);

            }
        }

        public async Task<ResponseService<RoleFindViewModel>> FindRole(string roleId)
        {
            var findRole = await _roleManager.FindByIdAsync(roleId);

            if (findRole == null)
                return ResponseService<RoleFindViewModel>.NotFound($"The role with id {roleId} doesn't exists");

            return ResponseService<RoleFindViewModel>.Ok(findRole.AsRoleEditDto());
        }

        public async Task<ResponseService<RoleFindViewModel>> UpdateRole(RoleFindViewModel updateRole)
        {
            var findRole = await _roleManager.FindByIdAsync(updateRole.RoleId);

            if (findRole == null)
                return ResponseService<RoleFindViewModel>.NotFound($"The role with id {updateRole} doesn't exists");

            try
            {
                findRole.Name = updateRole.RoleName;
                findRole.NormalizedName = updateRole.RoleName.ToUpper();

                var updateResult = await _roleManager.UpdateAsync(findRole);

                return ResponseService<RoleFindViewModel>.Ok(findRole.AsRoleEditDto());
            }
            catch (Exception ex)
            {
                return ResponseService<RoleFindViewModel>.ErrorMsg(ex.Message);
            }
        }

        public async Task<ResponseService<RoleFindViewModel>> DeleteRole(string id)
        {
            try
            {
                var findRole = await _roleManager.FindByIdAsync(id);

                if (findRole == null)
                    return ResponseService<RoleFindViewModel>.NotFound($"Role with id {id} does not exists");

                var resultDelete = await _roleManager.DeleteAsync(findRole);

                return ResponseService<RoleFindViewModel>.Deleted("Role has been deleted");
            }
            catch (Exception ex)
            {
                return ResponseService<RoleFindViewModel>.ExceptioThrow(ex.Message);
            }
        }

    }
}

