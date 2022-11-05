using MagazineManagment.DTO.ViewModels;
using MagazineManagment.Shared.Jwtbearer;
using MagazineManagment.Shared.UsersSeedValues;
using MagazineManagment.Web.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagazineManagment.Web.Controllers
{
    /// <summary>
    /// Login API Controller
    /// </summary>
    [ApiController]
    [Route("api/")]
    public class LoginController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _user;
        private readonly ILogger<LoginModel> _logger;
        private readonly JwtConfig _jwtConfig;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<IdentityUser> _userStore;

        /// <summary>
        /// Service  injection
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="jwtConfig"></param>
        /// <param name="signInManager"></param>
        /// <param name="user"></param>
        /// <param name="emailStore"></param>
        /// <param name="roleManager"></param>
        /// <param name="userStore"></param>
        public LoginController(ILogger<LoginModel> logger,
            IOptions<JwtConfig> jwtConfig,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> user,
            IUserStore<IdentityUser> userStore,
            RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _jwtConfig = jwtConfig.Value;
            _signInManager = signInManager;
            _user = user;
            _userStore = userStore;
            _roleManager = roleManager;
        }


        /// <summary>
        /// User Login 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(inputModel.Email, inputModel.Password, inputModel.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var AppUser = await _user.FindByEmailAsync(inputModel.Email);
                    var role = await _user.GetRolesAsync(AppUser);

                    var Token = GenerateToken(AppUser, role);

                    HttpContext.Session.SetString("Token", GenerateToken(AppUser, role));

                    _logger.LogInformation($"User logged in Token {Token}");

                    return Ok(Token);
                }
                else
                {
                    return BadRequest("Invalid login attempt.");
                }
            }
            return BadRequest("Invalid login  attempt");
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel Input)
        {
            if (ModelState.IsValid)
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

                    

                    var Token = GenerateToken(user, RoleName.Employee);

                    _logger.LogInformation(Token);

                    return Ok("User created" + " " + Token);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            return BadRequest("Server error...");
        }

        private string GenerateToken(IdentityUser user, IList<string> roles)
        {

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            return jwtTokenHandler.WriteToken(token);
        }

        private string GenerateToken(IdentityUser user, string role)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Key);
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
