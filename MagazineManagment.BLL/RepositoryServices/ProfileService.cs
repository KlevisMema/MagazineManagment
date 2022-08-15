using AutoMapper;
using MagazineManagment.BLL.RepositoryServices.ServiceInterfaces;
using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DAL.DataContext;
using MagazineManagment.DTO.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MagazineManagment.BLL.RepositoryServices
{
    public class ProfileService : IProfileService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _user;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProfileService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> user, ApplicationDbContext context, IMapper mapper)
        {
            _roleManager = roleManager;
            _user = user;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RolesGetAllDetails>> GetRoles()
        {
            var getRoles = await _roleManager.Roles.ToListAsync();
            return _mapper.Map<List<RolesGetAllDetails>>(getRoles);
        }

        public async Task<ResponseService<RoleCreateViewModel>> CreateRole(RoleCreateViewModel roleName)
        {
            try
            {
                IdentityRole identityRole = _mapper.Map<IdentityRole>(roleName);

                await _roleManager.CreateAsync(identityRole);

                return ResponseService<RoleCreateViewModel>.Ok(_mapper.Map<RoleCreateViewModel>(identityRole));
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


            return ResponseService<RoleFindViewModel>.Ok(_mapper.Map<RoleFindViewModel>(findRole));
        }

        public async Task<ResponseService<ProfileUpdateViewModel>> UpdateRole(ProfileUpdateViewModel updateRole)
        {
            var findRole = await _roleManager.FindByIdAsync(updateRole.RoleId);

            if (findRole == null)
                return ResponseService<ProfileUpdateViewModel>.NotFound($"The role with id {updateRole} doesn't exists");

            try
            {
                //findRole = _mapper.Map<IdentityRole>(updateRole);
                findRole.Name = updateRole.RoleName;
                findRole.NormalizedName = updateRole.RoleName.ToUpper();

                var updateResult = await _roleManager.UpdateAsync(findRole);

                return ResponseService<ProfileUpdateViewModel>.Ok(_mapper.Map<ProfileUpdateViewModel>(findRole));
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
            return ResponseService<RolesGetAllDetails>.Ok(_mapper.Map<RolesGetAllDetails>(findRole));
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


            return ResponseService<IEnumerable<UserInRoleViewModel>>.Ok(_mapper.Map<IEnumerable<UserInRoleViewModel>>(usersInRole));

        }

        public async Task<ResponseService<IEnumerable<UserNotInRoleViewModel>>> GettAllUsers(string id)
        {
            List<UserNotInRoleViewModel> usersNotInRole = new List<UserNotInRoleViewModel>();
            try
            {
                var role = await _roleManager.FindByIdAsync(id);

                if (role == null)
                    return ResponseService<IEnumerable<UserNotInRoleViewModel>>.NotFound("Role doesnt exists");

                var Users = await _context.Users.ToListAsync();

                foreach (var item in Users)
                {
                    var user = await _user.GetRolesAsync(item);
                    if (!user.Contains(role.Name))
                        usersNotInRole.Add(_mapper.Map<UserNotInRoleViewModel>(item));
                }

            }
            catch (Exception ex)
            {
                ResponseService<IEnumerable<UserNotInRoleViewModel>>.ExceptioThrow(ex.Message);
            }

            return ResponseService<IEnumerable<UserNotInRoleViewModel>>.Ok(usersNotInRole);

        }

        public async Task<ResponseService<IEnumerable<UserInRoleViewModel>>> AssignRoleToUsers(List<UserInRoleViewModel> users, string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                    return ResponseService<IEnumerable<UserInRoleViewModel>>.NotFound("Role doesn't exists");
                IdentityResult? assigningRoleResult = null;
                foreach (var item in users)
                {
                    var user = await _user.FindByIdAsync(item.UserId);

                    if (user == null)
                        return ResponseService<IEnumerable<UserInRoleViewModel>>.NotFound("User doesnt exists");
                    if (item.IsSelected && !(await _user.IsInRoleAsync(user, role.Name)))
                        assigningRoleResult = await _user.AddToRoleAsync(user, role.Name);
                    else
                    continue;
                }
                if (assigningRoleResult.Succeeded)
                    return ResponseService<IEnumerable<UserInRoleViewModel>>.Ok(users);
            }
            catch (Exception ex)
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