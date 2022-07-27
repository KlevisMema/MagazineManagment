using MagazineManagment.BLL.RepositoryServices.ServiceInterfaces;
using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DTO.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MagazineManagment.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RolesGetAllDetails>> GetRoles()
        {
            var roles = _profileService.GetRoles();
            return roles.ToList();
        }

        [HttpPost]
        public async Task<ActionResult<RoleCreateViewModel>> Index([FromBody] RoleCreateViewModel role)
        {
            var roleCreateResult = await _profileService.CreateRole(role);
            if (roleCreateResult.Success)
                return Ok(roleCreateResult.Value);

            return BadRequest(roleCreateResult.Message);
        }

        [HttpGet("FindRole/{roleId}")]
        public async Task<ActionResult<ResponseService<RoleFindViewModel>>> FindRole(string roleId)
        {
            var findRole = await _profileService.FindRole(roleId);
            if (findRole.Success)
                return Ok(findRole.Value);

            return BadRequest(findRole.Message);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseService<ProfileUpdateViewModel>>> UpdateRole([FromBody] ProfileUpdateViewModel roleId)
        {
            var resultUpdate = await _profileService.UpdateRole(roleId);

            if (resultUpdate.Success)
                return Ok(resultUpdate.Value);

            return BadRequest(resultUpdate.Message);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseService<RoleFindViewModel>>> DeleteRole([FromRoute] string id)
        {
            var resultDelete = await _profileService.DeleteRole(id);

            if (resultDelete.Success)
                return Ok(resultDelete.Message);

            return BadRequest(resultDelete.Message);
        }

        [HttpGet("GetRole/{id}")]
        public async Task<ActionResult<ResponseService<RolesGetAllDetails>>> GetAllDetailsOfARole(string id)
        {
            var findRole = await _profileService.GetRolesDetails(id);
            if (findRole.Success)
                return Ok(findRole.Value);

            return BadRequest(findRole.Message);
        }

        [HttpGet("GetUsersInRole/{id}")]
        public async Task<ActionResult<ResponseService<IEnumerable<UserInRoleViewModel>>>> GetUsersInRole(string id)
        {
            var usersInRole = await _profileService.GetUsersOfARole(id);
            if (usersInRole.Success)
                return Ok(usersInRole.Value);

            return BadRequest(usersInRole.Message);
        }

        [HttpGet("GetAllUsers/{id}")]
        public async Task<ActionResult<ResponseService<IEnumerable<UserInRoleViewModel>>>> GetAllUsers(string id)
        {
            var getAllUsersResult = await _profileService.GettAllUsers(id);
            if (getAllUsersResult.Success)
                return Ok(getAllUsersResult.Value);

            return BadRequest(getAllUsersResult.Message);
        }

        [HttpPost("AssignRoleToUsers/{id}")]
        public async Task<ActionResult<ResponseService<IEnumerable<UserInRoleViewModel>>>> AssignRoleToUsers(List<UserInRoleViewModel> users,[FromRoute] string id)
        {
            var asignResult =await _profileService.AssignRoleToUsers(users, id);
            if (asignResult.Success)
                return Ok(asignResult.Value);

            return BadRequest(asignResult.Message);
        }
    }
}
