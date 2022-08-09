﻿using MagazineManagment.DTO.ViewModels;
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
            IEnumerable<RolesGetAllDetails>? readResult = null;
            using (var client = new HttpClient())
            {
                var uri = _options.Value.ProfileGetOrDeleteProfile;
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);
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
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);
                resultPostRole = await client.PostAsJsonAsync(RequestDestination.ProfilePostOrEditRoleRoute, role);
                client.Dispose();
            }
            return resultPostRole;
        }

        public async Task<ProfileUpdateViewModel> GetEditRole(string id)
        {
            ProfileUpdateViewModel? getContent = null;
            using (var client = new HttpClient())
            {
                var uri = _options.Value.ProfileGetOrDeleteProfile;
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);
                var getCategoryResult = await client.GetAsync(RequestDestination.ProfileGetRoles + "/FindRole/" + id);
                getContent = await getCategoryResult.Content.ReadAsAsync<ProfileUpdateViewModel>();
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
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);
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
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);
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
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);
                deleteResult = await client.DeleteAsync(RequestDestination.ProfileGetRoles + "/" + id);
                client.Dispose();
            }
            return deleteResult;
        }

        public async Task<IEnumerable<UserInRoleViewModel>> GetAllUsersInRole(string id)
        {

            IEnumerable<UserInRoleViewModel>? readResult = null;
            using (var client = new HttpClient())
            {
                var uri = _options.Value.ProfileGetOrDeleteProfile;
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);
                var getCategoriesResponse = await client.GetAsync(RequestDestination.ProfileGetUsersInRole + id);

                readResult = await getCategoriesResponse.Content.ReadAsAsync<IList<UserInRoleViewModel>>();

                client.Dispose();
            }
            return readResult;
        }

        public async Task<IEnumerable<UserNotInRoleViewModel>> GetAllUsersNotInRole(string id)
        {
            IEnumerable<UserNotInRoleViewModel>? readResult = null;
            using (var client = new HttpClient())
            {
                var uri = _options.Value.ProfileGetOrDeleteProfile;
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);
                var getUsersResponse = await client.GetAsync(RequestDestination.ProfileGetRoles + "/GetAllUsers/" + id);

                if (getUsersResponse.IsSuccessStatusCode)
                    readResult = await getUsersResponse.Content.ReadAsAsync<IList<UserNotInRoleViewModel>>();
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
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);
                resultPostRoleToUsers = await client.PostAsJsonAsync(RequestDestination.ProfileAssignRoleToUsers + id, users);
                client.Dispose();
            }
            return resultPostRoleToUsers;
        }

        public async Task<HttpResponseMessage> RemoveRoleFromUsers(List<UserInRoleViewModel> users, string id)
        {
            HttpResponseMessage resultPostRoleToUsers = new();
            using (var client = new HttpClient())
            {
                var uri = _options.Value.ProfilePostOrEditRole;
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);
                resultPostRoleToUsers = await client.PostAsJsonAsync(RequestDestination.ProfileRemoveUsersFromRole + id, users);
                client.Dispose();
            }
            return resultPostRoleToUsers;
        }
    }
}