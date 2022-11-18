using AutoMapper;
using MagazineManagment.BLL.RepositoryServices.ServiceInterfaces;
using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DAL.DataContext;
using MagazineManagment.DTO.ViewModels;
using MagazineManagment.Shared.UsersSeedValues;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagazineManagment.BLL.RepositoryServices
{
    public class ProfileService : IProfileService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _user;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserStore<IdentityUser> _userStore;

        public ProfileService
        (
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> user,
            ApplicationDbContext context,
            IMapper mapper,
            SignInManager<IdentityUser> signInManager,
            IUserStore<IdentityUser> userStore
        )
        {
            _roleManager = roleManager;
            _user = user;
            _context = context;
            _mapper = mapper;
            _signInManager = signInManager;
            _userStore = userStore;
        }

        // Get all roles
        public async Task<IEnumerable<RolesGetAllDetails>> GetRoles()
        {
            var getRoles = await _roleManager.Roles.ToListAsync();
            return _mapper.Map<List<RolesGetAllDetails>>(getRoles);
        }

        // Create a role
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

        // Find a role by id
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

        // Update a role
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

        // Get role details by id
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

        // Delete a role 
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

        // Get all users of a role  
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

        // Get all users that are not in the role 
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

        // Assign a role to users
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
                    //if(await _user.IsInRoleAsync(user,"Admin"))
                    //    return ResponseService<IEnumerable<UserInRoleViewModel>>.ErrorMsg("You can not assing employee role to admin");
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

        // Remove users form a role
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
                    //if (await _user.IsInRoleAsync(user, "Admin"))
                    //    return ResponseService<IEnumerable<UserInRoleViewModel>>.ErrorMsg("You can not remove admin  role");
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

        // User Login 
        public async Task<ResponseService<Tuple<string?, IdentityUser?>>> Login(LoginViewModel inputModel, string key)
        {
            var result = await _signInManager.PasswordSignInAsync(inputModel.Email, inputModel.Password, inputModel.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var AppUser = await _user.FindByEmailAsync(inputModel.Email);
                var role = await _user.GetRolesAsync(AppUser);

                var Token = GenerateToken(AppUser, role, key);

                return ResponseService<Tuple<string?, IdentityUser?>>.Ok(Tuple.Create(Token, AppUser));
            }
            return ResponseService<Tuple<string?, IdentityUser?>>.ErrorMsg("Invalid login attempt");
        }

        // Register
        public async Task<ResponseService<string>> Register(RegisterViewModel Input, string key)
        {
            var user = CreateUser();

            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);

            user.Email = Input.Email;
            var result = await _user.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                var findRole = await _roleManager.FindByNameAsync(RoleName.Employee);
                if (findRole != null)
                    await _user.AddToRoleAsync(user, RoleName.Employee);

                var Token = GenerateToken(user, RoleName.Employee, key);

                return ResponseService<string>.Ok(Token);
            }
            else
            {
                return ResponseService<string>.ErrorMsg("Server error...");
            }
        }

        private string GenerateToken(IdentityUser user, IList<string> roles, string secretKey)
        {

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            return jwtTokenHandler.WriteToken(token);
        }

        private string GenerateToken(IdentityUser user, string role, string secretKey)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role,role),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}