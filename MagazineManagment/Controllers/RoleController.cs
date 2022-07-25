using MagazineManagment.BLL.RepositoryServices;
using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DTO.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MagazineManagment.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IProfileService _profileService;
        public RoleController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RoleCreateViewModel>> GetRoles()
        {
            var roles = _profileService.GetRoles();
            return roles.ToList();
        }

        [HttpPost]
        public async Task<ActionResult<IdentityResult>> Index(RoleCreateViewModel role)
        {
            var roleCreateResult = await _profileService.CreateRole(role);
            if (roleCreateResult.Success)
                return Ok(role);

            return BadRequest();
        }

        [HttpGet("FindRole/{roleId}")]
        public async Task<ActionResult<ResponseService<RoleFindViewModel>>> FindRole([FromRoute] string roleId)
        {
            var findRole = await _profileService.FindRole(roleId);
            if (findRole.Success)
                return Ok(findRole.Value);

            return BadRequest(findRole.Message);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseService<RoleFindViewModel>>> UpdateRole([FromBody] RoleFindViewModel roleId)
        {
            var resultUpdate = await _profileService.UpdateRole(roleId);

            if (resultUpdate.Success)
                return Ok(resultUpdate.Value);

            return BadRequest(resultUpdate.Message);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseService<RoleFindViewModel>>> DeleteRole(string id)
        {
            var resultDelete = await _profileService.DeleteRole(id);

            if(resultDelete.Success)
                return Ok(resultDelete.Message);

            return BadRequest(resultDelete.Message);
        }
    }
}
