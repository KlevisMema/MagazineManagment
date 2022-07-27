using MagazineManagment.DTO.ViewModels;
using MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagazineManagment.ClientApplication.Controllers
{
    public class ProfileClientController : Controller
    {
        private readonly IProfileApiCalls _profileApiCalls;
        public ProfileClientController(IProfileApiCalls profileApiCalls)
        {
            _profileApiCalls = profileApiCalls;
        }

        public async Task<IActionResult> Index()
        {
            var getAllRoles = await _profileApiCalls.GetAllRoles();
            return View(getAllRoles);
        }

        [Authorize(Roles = "Admini")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleCreateViewModel role)
        {
            if (ModelState.IsValid)
            {
                var postRoleResult = await _profileApiCalls.PostCreateRole(role);
                if (postRoleResult.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                ModelState.AddModelError(string.Empty, await postRoleResult.Content.ReadAsStringAsync());
            }
            return View(role);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var getRoleToEditResult = await _profileApiCalls.GetEditRole(id);
            if (getRoleToEditResult != null)
            {
                return View(getRoleToEditResult);
            }
            return NotFound("Role doesnt exists");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProfileUpdateViewModel role)
        {
            if (ModelState.IsValid)
            {
                var postUpdateResult = await _profileApiCalls.PostUpdateRole(role);
                if (postUpdateResult.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                ModelState.AddModelError(string.Empty, await postUpdateResult.Content.ReadAsStringAsync());
            }

            return View(role);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var getRoleToDeleteResult = await _profileApiCalls.GetAllRolesDetails(id);
            if (getRoleToDeleteResult != null)
            {
                return View(getRoleToDeleteResult);
            }
            return NotFound("Role doesnt exists");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(RolesGetAllDetails role)
        {
            var getRoleToDeleteResult = await _profileApiCalls.DeleteRole(role.RoleId);
            if (getRoleToDeleteResult.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError(string.Empty, await getRoleToDeleteResult.Content.ReadAsStringAsync());
            return View(role);
        }

        [HttpGet]
        public  async Task<IActionResult> Users(string id)
        {
            var getUsersInRole = await _profileApiCalls.UsersInRole(id);
            if (getUsersInRole == null)
                return NotFound();
            return View(getUsersInRole);
        }

        [HttpGet]
        public async Task<IActionResult> AsingRoleToUsers(string id)
        {
            var getUsers = await _profileApiCalls.GetAllUsers(id);
            ViewBag.roleId = id;
            return View(getUsers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AsingRoleToUsers(List<UserInRoleViewModel> users, string id)
        {
            var asignRoleToUsersResult = await _profileApiCalls.AssignRoleToUsers(users, id);
            if (asignRoleToUsersResult.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError(string.Empty,await asignRoleToUsersResult.Content.ReadAsStringAsync());
            return RedirectToAction("AsingRoleToUsers");
        }
    }
}
