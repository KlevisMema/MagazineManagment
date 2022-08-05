using MagazineManagment.BLL.RepositoryServices.ServiceInterfaces;
using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DTO.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagazineManagment.Web.Controllers
{
    /// <summary>
    /// Profile controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        /// <summary>
        /// Inject profile service  
        /// </summary>
        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }
        /// <summary>
        /// Get all roles
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RolesGetAllDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<RolesGetAllDetails>>> GetRoles()
        {
            var roles = await _profileService.GetRoles();
            return roles.ToList();
        }
        /// <summary>
        /// Create a role
        /// </summary>
        /// <param name="role"></param>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleCreateViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RoleCreateViewModel>> Index([FromBody] RoleCreateViewModel role)
        {
            var roleCreateResult = await _profileService.CreateRole(role);
            if (roleCreateResult.Success)
                return Ok(roleCreateResult.Value);

            return BadRequest(roleCreateResult.Message);
        }
        /// <summary>
        /// Find a role by id 
        /// </summary>
        /// <param name="roleId"></param>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin")]
        [HttpGet("FindRole/{roleId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleFindViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResponseService<RoleFindViewModel>>> FindRole(string roleId)
        {
            var findRole = await _profileService.FindRole(roleId);
            if (findRole.Success)
                return Ok(findRole.Value);

            return BadRequest(findRole.Message);
        }
        /// <summary>
        /// Update a role 
        /// </summary>
        /// <param name="roleId"></param>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProfileUpdateViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResponseService<ProfileUpdateViewModel>>> UpdateRole([FromBody] ProfileUpdateViewModel roleId)
        {
            var resultUpdate = await _profileService.UpdateRole(roleId);

            if (resultUpdate.Success)
                return Ok(resultUpdate.Value);

            return BadRequest(resultUpdate.Message);
        }
        /// <summary>
        /// Delete a  role
        /// </summary>
        /// <param name="id"></param>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleFindViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResponseService<RoleFindViewModel>>> DeleteRole([FromRoute] string id)
        {
            var resultDelete = await _profileService.DeleteRole(id);

            if (resultDelete.Success)
                return Ok(resultDelete.Message);

            return BadRequest(resultDelete.Message);
        }
        /// <summary>
        /// Get all details of a role  
        /// </summary>
        /// <param name="id"></param>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin")]
        [HttpGet("GetRole/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RolesGetAllDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResponseService<RolesGetAllDetails>>> GetAllDetailsOfARole(string id)
        {
            var findRole = await _profileService.GetRolesDetails(id);
            if (findRole.Success)
                return Ok(findRole.Value);

            return BadRequest(findRole.Message);
        }
        /// <summary>
        /// Get all users of a role
        /// </summary>
        /// <param name="id"></param>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin")]
        [HttpGet("GetUsersOfARole/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserInRoleViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResponseService<IEnumerable<UserInRoleViewModel>>>> GetUsersOfARole(string id)
        {
            var usersInRole = await _profileService.GetUsersOfARole(id);
            if (usersInRole.Success)
                return Ok(usersInRole.Value);

            return BadRequest(usersInRole.Message);
        }
        /// <summary>
        ///  Get all users
        /// </summary>
        /// <param name="id"></param>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserNotInRoleViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResponseService<IEnumerable<UserNotInRoleViewModel>>>> GetAllUsers([FromRoute] string id)
        {
            var getAllUsersResult = await _profileService.GettAllUsers(id);
            if (getAllUsersResult.Success)
                return Ok(getAllUsersResult.Value);

            return BadRequest(getAllUsersResult.Message);
        }
        /// <summary>
        /// Assign a role to a user
        /// </summary>
        /// <param name="users"></param>
        /// <param name="id"></param>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin")]
        [HttpPost("AssignRoleToUsers/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserInRoleViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResponseService<IEnumerable<UserInRoleViewModel>>>> AssignRoleToUsers(List<UserInRoleViewModel> users, [FromRoute] string id)
        {
            var asignResult = await _profileService.AssignRoleToUsers(users, id);
            if (asignResult.Success)
                return Ok(asignResult.Value);

            return BadRequest(asignResult.Message);
        }
        /// <summary>
        /// Remove users from a role
        /// </summary>
        /// <param name="users"></param>
        /// <param name="id"></param>
        /// <response code="401"> Unauthorized </response>
        [Authorize(Roles = "Admin")]
        [HttpPost("RemoveRoleFromUsers/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserInRoleViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResponseService<IEnumerable<UserInRoleViewModel>>>> RemoveRoleFromUsers(List<UserInRoleViewModel> users, [FromRoute] string id)
        {
            var asignResult = await _profileService.RemoveUsersFromRole(users, id);
            if (asignResult.Success)
                return Ok(asignResult.Value);

            return BadRequest(asignResult.Message);
        }
    }
}
