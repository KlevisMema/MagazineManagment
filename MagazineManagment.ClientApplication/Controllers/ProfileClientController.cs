using MagazineManagment.DTO.ViewModels;
using MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MagazineManagment.ClientApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProfileClientController : Controller
    {
        private readonly IProfileApiCalls _profileApiCalls;
        private readonly SignInManager<IdentityUser> _signInManager;
        public ProfileClientController(IProfileApiCalls profileApiCalls, SignInManager<IdentityUser> signInManager)
        {
            _profileApiCalls = profileApiCalls;
            _signInManager = signInManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var getAllRoles = await _profileApiCalls.GetAllRoles();
            return View(getAllRoles);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var getRoleToEditResult = await _profileApiCalls.GetEditRole(id);
            if (getRoleToEditResult.RoleId is null)
                return NotFound();
            return View(getRoleToEditResult);
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var getRoleToDeleteResult = await _profileApiCalls.GetAllRolesDetails(id);

            if (getRoleToDeleteResult.RoleName is null)
                return NotFound();

            return View(getRoleToDeleteResult);
            
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Users(string id)
        {
            var getUsersInRole = await _profileApiCalls.GetAllUsersInRole(id);
            if (getUsersInRole == null)
                return NotFound();
            return View(getUsersInRole);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AsingRoleToUsers(string id)
        {
            var getUsers = await _profileApiCalls.GetAllUsersNotInRole(id);
            if (getUsers is null)
                return NotFound();
            ViewBag.roleId = id;
            return View(getUsers);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AsingRoleToUsers(List<UserInRoleViewModel> users, string id)
        {
            var asignRoleToUsersResult = await _profileApiCalls.AssignRoleToUsers(users, id);
            if (asignRoleToUsersResult.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError(string.Empty, await asignRoleToUsersResult.Content.ReadAsStringAsync());
            return RedirectToAction("AsingRoleToUsers");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> RemoveRoleFromUsers(string id)
        {
            var getUsers = await _profileApiCalls.GetAllUsersInRole(id);

            if (getUsers is null)
                return NotFound();

            ViewBag.roleId = id;
            return View(getUsers);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRoleFromUsers(List<UserInRoleViewModel> users, string id)
        {
            var removeRoleFromUsersResult = await _profileApiCalls.RemoveRoleFromUsers(users, id);
            if (removeRoleFromUsersResult.IsSuccessStatusCode)
                //await _signInManager.SignOutAsync();
                return RedirectToAction("Index");



            ModelState.AddModelError(string.Empty, await removeRoleFromUsersResult.Content.ReadAsStringAsync());
            return RedirectToAction("RemoveRoleFromUsers");
        }
    }
}