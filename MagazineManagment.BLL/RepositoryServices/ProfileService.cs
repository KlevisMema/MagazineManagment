using MagazineManagment.BLL.RepositoryServices.ServiceInterfaces;
using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DAL.DataContext;
using MagazineManagment.DTO.DataTransferObjects;
using MagazineManagment.DTO.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MagazineManagment.BLL.RepositoryServices
{
    public class ProfileService : IProfileService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser>        _user;
        private readonly ApplicationDbContext          _context;

        public ProfileService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> user, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _user = user;
            _context = context;
        }

        public IEnumerable<RolesGetAllDetails> GetRoles()
        {
            var getRoles = _roleManager.Roles.ToList();
            return getRoles.Select(roles => roles.AsRoleGetAllDetailsDto());
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
            IdentityRole? findRole = null;

            try
            {
                findRole = await _roleManager.FindByIdAsync(roleId);

                if (findRole == null)
                    return ResponseService<RoleFindViewModel>.NotFound($"The role with id {roleId} doesn't exists");
            }
            catch (Exception ex)
            {
                ResponseService<RoleFindViewModel>.ExceptioThrow(ex.Message);
            }
            

            return ResponseService<RoleFindViewModel>.Ok(findRole.AsRoleEditDto());
        }

        public async Task<ResponseService<ProfileUpdateViewModel>> UpdateRole(ProfileUpdateViewModel updateRole)
        {
            var findRole = await _roleManager.FindByIdAsync(updateRole.RoleId);

            if (findRole == null)
                return ResponseService<ProfileUpdateViewModel>.NotFound($"The role with id {updateRole} doesn't exists");

            try
            {
                findRole.Name = updateRole.RoleName;
                findRole.NormalizedName = updateRole.RoleName.ToUpper();

                var updateResult = await _roleManager.UpdateAsync(findRole);

                return ResponseService<ProfileUpdateViewModel>.Ok(findRole.AsRoleUpdateDto());
            }
            catch (Exception ex)
            {
                return ResponseService<ProfileUpdateViewModel>.ErrorMsg(ex.Message);
            }
        }

        public async Task<ResponseService<RolesGetAllDetails>> GetRolesDetails(string id)
        {
            IdentityRole? findRole = null;
            try
            {
                findRole = await _roleManager.FindByIdAsync(id);

                if (findRole == null)
                    return ResponseService<RolesGetAllDetails>.NotFound($"The role with id {id} doesn't exists");
            }
            catch (Exception ex)
            {
                ResponseService<RolesGetAllDetails>.ExceptioThrow(ex.Message);
            }
            return ResponseService<RolesGetAllDetails>.Ok(findRole.AsRoleGetAllDetailsDto());
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

        public async Task<ResponseService<IEnumerable<UserInRoleViewModel>>> GetUsersOfARole(string roleId)
        {
            IList<IdentityUser>? usersInRole = null;
            try
            {
                var role = await _roleManager.FindByIdAsync(roleId);

                if (role == null)
                    return ResponseService<IEnumerable<UserInRoleViewModel>>.NotFound("Role doesn't exists");

                usersInRole = await _user.GetUsersInRoleAsync(role.Name);

                if (usersInRole == null)
                    return ResponseService<IEnumerable<UserInRoleViewModel>>.NotFound("No users Found");
            }
            catch (Exception ex)
            {
                return ResponseService<IEnumerable<UserInRoleViewModel>>.ExceptioThrow(ex.Message);
            }
            

            return ResponseService<IEnumerable<UserInRoleViewModel>>.Ok(usersInRole.Select(u => u.AsUserInRoleDto()));

        }

        public async Task<ResponseService<IEnumerable<UserInRoleViewModel>>> GettAllUsers(string id)
        {
            List<UserInRoleViewModel> usersNotInRole = new List<UserInRoleViewModel>();
            try
            {
                var role = await _roleManager.FindByIdAsync(id);

                if (role == null)
                    return ResponseService<IEnumerable<UserInRoleViewModel>>.NotFound("Role doesnt exists");

                var Users = await _context.Users.ToListAsync();

                foreach (var item in Users)
                {
                    var user = await _user.GetRolesAsync(item);
                    if (!user.Contains(role.Name))
                        usersNotInRole.Add(item.AsUserNotInRoleDto());
                }
            }
            catch (Exception ex)
            {
                ResponseService<IEnumerable<UserInRoleViewModel>>.ExceptioThrow(ex.Message);
            }
            
            return ResponseService<IEnumerable<UserInRoleViewModel>>.Ok(usersNotInRole);
            
        }

        public async Task<ResponseService<IEnumerable<UserInRoleViewModel>>> AssignRoleToUsers(List<UserInRoleViewModel> users, string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                    return ResponseService<IEnumerable<UserInRoleViewModel>>.NotFound("Role doesn't exists");


                foreach (var item in users)
                {
                    var user = await _user.FindByIdAsync(item.UserId);

                    if (user == null)
                        return ResponseService<IEnumerable<UserInRoleViewModel>>.NotFound("User doesnt exists");
                    IdentityResult? assigningRoleResult = null;
                    //IdentityResult? claimResult = null;
                    if (item.IsSelected && !(await _user.IsInRoleAsync(user, role.Name)))
                    {
                        assigningRoleResult = await _user.AddToRoleAsync(user, role.Name);
                        //claimResult = await _user.AddClaimAsync(user, new Claim("Role", role.Name));
                    }
                    else
                        continue;
                    if (assigningRoleResult.Succeeded /*&& claimResult.Succeeded*/)
                        return ResponseService<IEnumerable<UserInRoleViewModel>>.Ok(users);
                }
            }
            catch (Exception  ex)
            {
                return ResponseService<IEnumerable<UserInRoleViewModel>>.ExceptioThrow(ex.Message);
            }

            return ResponseService<IEnumerable<UserInRoleViewModel>>.ErrorMsg("Could not assign role to users");
        }

        public async Task<ResponseService<IEnumerable<UserInRoleViewModel>>> RemoveUsersFromRole(List<UserInRoleViewModel> users, string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                    return ResponseService<IEnumerable<UserInRoleViewModel>>.NotFound("Role doesn't exists");

                IdentityUser? user = null;
                IdentityResult assigningRoleResult = new();

                foreach (var item in users)
                {
                    user = await _user.FindByIdAsync(item.UserId);

                    if (user == null)
                        return ResponseService<IEnumerable<UserInRoleViewModel>>.NotFound("User doesnt exists");
                    if (!item.IsSelected && await _user.IsInRoleAsync(user, role.Name))
                        assigningRoleResult = await _user.RemoveFromRoleAsync(user, role.Name);
                }
                if (assigningRoleResult.Succeeded)
                    return ResponseService<IEnumerable<UserInRoleViewModel>>.Deleted($"Users successfully removed from {role.Name} role");
            }
            catch (Exception ex)
            {
                ResponseService<UserInRoleViewModel>.ExceptioThrow(ex.Message);
            }
            return ResponseService<IEnumerable<UserInRoleViewModel>>.Ok(users);
        }
    }
}