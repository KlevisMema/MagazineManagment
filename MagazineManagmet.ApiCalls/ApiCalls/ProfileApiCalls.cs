using MagazineManagment.DTO.ViewModels;
using MagazineManagment.Shared.ApiUrlDestinations;
using MagazineManagment.Shared.Jwtbearer;
using MagazineManagment.Web.ApiCalls.ApiUrlValues;
using MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

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
            var client = new HttpClient();
            var uri = _options.Value.ProfileGetOrDeleteProfile;

            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);

            var getCategoriesResponse = await client.GetAsync(RequestDestination.ProfileGetRoles);
            var readResult = await getCategoriesResponse.Content.ReadAsAsync<IList<RolesGetAllDetails>>();

            client.Dispose();
            return readResult;
        }

        public async Task<HttpResponseMessage> PostCreateRole(RoleCreateViewModel role)
        {
            var client = new HttpClient();
            var uri = _options.Value.ProfilePostOrEditRole;

            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);

            var resultPostRole = await client.PostAsJsonAsync(RequestDestination.ProfilePostOrEditRoleRoute, role);

            client.Dispose();
            return resultPostRole;
        }

        public async Task<ProfileUpdateViewModel> GetEditRole(string id)
        {
            var client = new HttpClient();
            var uri = _options.Value.ProfileGetOrDeleteProfile;

            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);

            var getCategoryResult = await client.GetAsync(RequestDestination.ProfileGetRoles + "/FindRole/" + id);
            var getContent = await getCategoryResult.Content.ReadAsAsync<ProfileUpdateViewModel>();

            client.Dispose();
            return getContent;
        }

        public async Task<HttpResponseMessage> PostUpdateRole(ProfileUpdateViewModel role)
        {
            var client = new HttpClient();
            var uri = _options.Value.ProfilePostOrEditRole;

            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);

            var getPostEditResult = await client.PutAsJsonAsync(RequestDestination.ProfilePostOrEditRoleRoute, role);

            client.Dispose();
            return getPostEditResult;
        }

        public async Task<RolesGetAllDetails> GetAllRolesDetails(string id)
        {
            var client = new HttpClient();
            var uri = _options.Value.ProfileGetOrDeleteProfile;

            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);

            var getCategoryResult = await client.GetAsync(RequestDestination.ProfileGetRoleDetailsRoute + id);
            var getContent = await getCategoryResult.Content.ReadAsAsync<RolesGetAllDetails>();

            client.Dispose();
            return getContent;
        }

        public async Task<HttpResponseMessage> DeleteRole(string id)
        {
            var client = new HttpClient();
            var uri = _options.Value.ProfileGetOrDeleteProfile;

            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);

            var deleteResult = await client.DeleteAsync(RequestDestination.ProfileGetRoles + "/" + id);

            client.Dispose();
            return deleteResult;
        }

        public async Task<IEnumerable<UserInRoleViewModel>> GetAllUsersInRole(string id)
        {
            IEnumerable<UserInRoleViewModel>? readResult = null;
            var client = new HttpClient();
            var uri = _options.Value.ProfileGetOrDeleteProfile;

            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);

            var getCategoriesResponse = await client.GetAsync(RequestDestination.ProfileGetUsersInRole + id);

            if (getCategoriesResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                return readResult;

            readResult = await getCategoriesResponse.Content.ReadAsAsync<IEnumerable<UserInRoleViewModel>>();

            client.Dispose();
            return readResult;
        }

        public async Task<IEnumerable<UserNotInRoleViewModel>> GetAllUsersNotInRole(string id)
        {
            IEnumerable<UserNotInRoleViewModel>? readResult = null;
            var client = new HttpClient();
            var uri = _options.Value.ProfileGetOrDeleteProfile;

            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);

            var getUsersResponse = await client.GetAsync(RequestDestination.ProfileGetRoles + "/GetAllUsers/" + id);

            if (getUsersResponse.IsSuccessStatusCode)
                readResult = await getUsersResponse.Content.ReadAsAsync<IList<UserNotInRoleViewModel>>();

            client.Dispose();
            return readResult;
        }

        public async Task<HttpResponseMessage> AssignRoleToUsers(List<UserInRoleViewModel> users, string id)
        {
            var client = new HttpClient();
            var uri = _options.Value.ProfilePostOrEditRole;

            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);

            var resultPostRoleToUsers = await client.PostAsJsonAsync(RequestDestination.ProfileAssignRoleToUsers + id, users);
            
            client.Dispose();
            return resultPostRoleToUsers;
        }

        public async Task<HttpResponseMessage> RemoveRoleFromUsers(List<UserInRoleViewModel> users, string id)
        {
            var client = new HttpClient();
            var uri = _options.Value.ProfilePostOrEditRole;

            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);

            var resultPostRoleToUsers = await client.PostAsJsonAsync(RequestDestination.ProfileRemoveUsersFromRole + id, users);

            client.Dispose();
            return resultPostRoleToUsers;
        }
    }
}