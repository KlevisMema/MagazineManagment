using MagazineManagment.BLL.RepositoryServices.ServiceInterfaces;
using MagazineManagment.DTO.ViewModels;
using MagazineManagment.Web.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MagazineManagment.Web.Controllers
{
    /// <summary>
    /// Login API Controller
    /// </summary>
    [ApiController]
    [Route("api/")]
    public class LoginController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly IOptions<JwtConfig> _jwt;

        /// <summary>
        /// Service  injection
        /// </summary>
        /// <param name="profileService"></param>
        /// <param name="jwt"></param>
        public LoginController
        (
            IProfileService profileService, 
            IOptions<JwtConfig> jwt
        )
        {
            _profileService = profileService;
            _jwt = jwt;
        }


        /// <summary>
        /// User Login 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<Tuple<string?, IdentityUser?>>> Login
        (
            [FromForm] LoginViewModel inputModel
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);   

            var result = await _profileService.Login(inputModel, _jwt.Value.Key);

            if (result.Success)
                return Ok(result.Value);

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register
        (
            [FromForm] RegisterViewModel Input
        )
        {
            if (ModelState.IsValid)
            {
               
            }
            var register = await _profileService.Register(Input, _jwt.Value.Key);

            if (register.Success)
                return Ok(register.Value);

            return BadRequest(register.Message);
        }
    }
}