using MagazineManagment.DTO.ViewModels;
using MagazineManagment.Shared.ApiUrlDestinations;
using MagazineManagment.Shared.MediaFormatter;
using MagazineManagment.Web.ApiCalls.ApiUrlValues;
using MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces;
using Microsoft.Extensions.Options;

namespace MagazineManagmet.ApiCalls.ApiCalls
{
    public class ProfileApiCalls : IProfileApiCalls
    {
        private readonly IOptions<FetchApiValue> _options;
        public ProfileApiCalls(IOptions<FetchApiValue> options)
        {
            _options = options;
        }

        public async Task<IEnumerable<RolesGetAllDetails>> GetAllRoles()
        {
            IEnumerable<RolesGetAllDetails>? readResult = null;
            using (var client = new HttpClient())
            {
                var uri = _options.Value.ProfileGetOrDeleteProfile;
                client.BaseAddress = new Uri(uri);
                var getCategoriesResponse = await client.GetAsync(RequestDestination.ProfileGetRoles);

                readResult = await getCategoriesResponse.Content.ReadAsAsync<IList<RolesGetAllDetails>>();
                client.Dispose();
            }
            return readResult;
        }

        public async Task<HttpResponseMessage> PostCreateRole(RoleCreateViewModel role)
        {
            HttpResponseMessage resultPostRole = new();
            using (var client = new HttpClient())
            {
                var uri = _options.Value.ProfilePostOrEditRole;
                client.BaseAddress = new Uri(uri);
                resultPostRole = await client.PostAsJsonAsync(RequestDestination.ProfilePostOrEditRoleRoute, role);
                client.Dispose();
            }
            return resultPostRole;
        }

        public async Task<RoleFindViewModel> GetEditRole(string id)
        {
            RoleFindViewModel? getContent = null;
            using (var client = new HttpClient())
            {
                var uri = _options.Value.ProfileGetOrDeleteProfile;
                client.BaseAddress = new Uri(uri);
                var getCategoryResult = await client.GetAsync(RequestDestination.ProfileGetRoles + "/FindRole/" + id);
                getContent = await getCategoryResult.Content.ReadAsAsync<RoleFindViewModel>();
                client.Dispose();
            }
            return getContent;
        }

        public async Task<HttpResponseMessage> PostUpdateRole(ProfileUpdateViewModel role)
        {
            HttpResponseMessage? getPostEditResult = null;
            using (var client = new HttpClient())
            {
                var uri = _options.Value.ProfilePostOrEditRole;
                client.BaseAddress = new Uri(uri);
                getPostEditResult = await client.PutAsJsonAsync(RequestDestination.ProfilePostOrEditRoleRoute, role);
                client.Dispose();
            }
            return getPostEditResult;
        }

        public async Task<RolesGetAllDetails> GetAllRolesDetails(string id)
        {
            RolesGetAllDetails? getContent = null;
            using (var client = new HttpClient())
            {
                var uri = _options.Value.ProfileGetOrDeleteProfile;
                client.BaseAddress = new Uri(uri);
                var getCategoryResult = await client.GetAsync(RequestDestination.ProfileGetRoleDetailsRoute + id);
                getContent = await getCategoryResult.Content.ReadAsAsync<RolesGetAllDetails>();
                client.Dispose();
            }
            return getContent;
        }

        public async Task<HttpResponseMessage> DeleteRole(string id)
        {
            HttpResponseMessage? deleteResult = null;
            using (var client = new HttpClient())
            {
                var uri = _options.Value.ProfileGetOrDeleteProfile;
                client.BaseAddress = new Uri(uri);
                deleteResult = await client.DeleteAsync(RequestDestination.ProfileGetRoles + "/" + id);
                client.Dispose();
            }
            return deleteResult;
        }

        public async Task<IEnumerable<UserInRoleViewModel>> UsersInRole(string id)
        {

            IEnumerable<UserInRoleViewModel>? readResult = null;
            using (var client = new HttpClient())
            {
                var uri = _options.Value.ProfileGetOrDeleteProfile;
                client.BaseAddress = new Uri(uri);
                var getCategoriesResponse = await client.GetAsync(RequestDestination.ProfileGetUsersInRole + id);

                readResult = await getCategoriesResponse.Content.ReadAsAsync<IList<UserInRoleViewModel>>();

                client.Dispose();
            }
            return readResult;
        }

        public async Task<IEnumerable<UserInRoleViewModel>> GetAllUsers(string id)
        {
            IEnumerable<UserInRoleViewModel>? readResult = null;
            using (var client = new HttpClient())
            {
                var uri = _options.Value.ProfileGetOrDeleteProfile;
                client.BaseAddress = new Uri(uri);
                var getUsersResponse = await client.GetAsync(RequestDestination.ProfileGetRoles + "/GetAllUsers/" + id);

                if (getUsersResponse.IsSuccessStatusCode)
                    readResult = await getUsersResponse.Content.ReadAsAsync<IList<UserInRoleViewModel>>();
                //else
                //    readResult.Clear();
                
            }
            return readResult;
        }

        public async Task<HttpResponseMessage> AssignRoleToUsers(List<UserInRoleViewModel> users, string id)
        {
            HttpResponseMessage resultPostRoleToUsers = new();
            using (var client = new HttpClient())
            {
                var uri = _options.Value.ProfilePostOrEditRole;
                client.BaseAddress = new Uri(uri);
                resultPostRoleToUsers = await client.PostAsJsonAsync(RequestDestination.ProfileAssignRoleToUsers + id, users);
                client.Dispose();
            }
            return resultPostRoleToUsers;
        }
    }
}
