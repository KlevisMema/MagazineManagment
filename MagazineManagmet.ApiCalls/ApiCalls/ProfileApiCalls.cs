﻿using MagazineManagment.DTO.ViewModels;
using MagazineManagment.Shared.ApiUrlDestinations;
using MagazineManagment.Web.ApiCalls.ApiUrlValues;
using MagazineManagmet.ApiCalls.ApiCall.GenericApiCall;
using MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces;
using Microsoft.Extensions.Options;

namespace MagazineManagmet.ApiCalls.ApiCalls
{
    public class ProfileApiCalls : IProfileApiCalls
    {
        private readonly IOptions<FetchApiValue> _options;
        private readonly IGenericApi<RolesGetAllDetails> _roles;
        private readonly IGenericApi<RoleCreateViewModel> _roleCreate;
        private readonly IGenericApi<ProfileUpdateViewModel> _roleEdit;
        private readonly IGenericApi<RolesGetAllDetails> _roleDetails;
        private readonly IGenericApi<UserInRoleViewModel> _inRoleDetails;
        private readonly IGenericApi<UserNotInRoleViewModel> _notInRoleDetails;
        public ProfileApiCalls(IOptions<FetchApiValue> options, IGenericApi<RolesGetAllDetails> roles,
            IGenericApi<RoleCreateViewModel> roleCreate, IGenericApi<ProfileUpdateViewModel> roleEdit, 
            IGenericApi<RolesGetAllDetails> roleDetails, IGenericApi<UserInRoleViewModel> inRoleDetails,
            IGenericApi<UserNotInRoleViewModel> notInRoleDetails)
        {
            _options = options;
            _roles = roles;
            _roleCreate = roleCreate;
            _roleEdit = roleEdit;
            _roleDetails = roleDetails;
            _inRoleDetails = inRoleDetails;
            _notInRoleDetails = notInRoleDetails;
        }

        public async Task<IEnumerable<RolesGetAllDetails>> GetAllRoles()
        {
            _roles.DefaultRoute = RequestDestination.ProfileGetRoles;
            _roles.Uri = _options.Value.GetDeleteDefault;
            return await _roles.GetAllRecords(String.Empty);
        }

        public async Task<HttpResponseMessage> PostCreateRole(RoleCreateViewModel role)
        {
            _roleCreate.DefaultRoute = RequestDestination.ProfilePostOrEditRoleRoute;
            _roleCreate.Uri = _options.Value.CreateEditDefault;
            return await _roleCreate.PostRecord(role);
        }

        public async Task<ProfileUpdateViewModel> GetEditRole(string id)
        {
            _roleEdit.DefaultRoute = RequestDestination.ProfileGetRole;
            _roleEdit.Uri = _options.Value.GetDeleteDefault;
            return await _roleEdit.RecordDetails(id);
        }

        public async Task<HttpResponseMessage> PostUpdateRole(ProfileUpdateViewModel role)
        {
            _roleEdit.DefaultRoute = RequestDestination.ProfilePostOrEditRoleRoute;
            _roleEdit.Uri = _options.Value.CreateEditDefault;
            return await _roleEdit.Edit(role);
        }

        public async Task<RolesGetAllDetails> GetAllRolesDetails(string id)
        {

            _roleDetails.DefaultRoute = RequestDestination.ProfileGetRoleDetailsRoute;
            _roleDetails.Uri = _options.Value.GetDeleteDefault;
            return await _roleDetails.RecordDetails(id);
        }

        public async Task<HttpResponseMessage> DeleteRole(string id)
        {
            _roles.DefaultRoute = RequestDestination.ProfileGetRoles;
            _roles.Uri = _options.Value.GetDeleteDefault;
            return await _roles.Delete(id);
        }

        public async Task<IEnumerable<UserInRoleViewModel>> GetAllUsersInRole(string id)
        {
            _inRoleDetails.DefaultRoute = RequestDestination.ProfileGetUsersInRole;
            _inRoleDetails.Uri = _options.Value.GetDeleteDefault;
            return await _inRoleDetails.GetRecordsDetailsById(id);
        }

        public async Task<IEnumerable<UserNotInRoleViewModel>> GetAllUsersNotInRole(string id)
        {
            _notInRoleDetails.DefaultRoute = RequestDestination.ProfileGetRoles + "/GetAllUsers/";
            _notInRoleDetails.Uri = _options.Value.GetDeleteDefault;
            return await _notInRoleDetails.GetRecordsDetailsById(id);
        }

        public async Task<HttpResponseMessage> AssignRoleToUsers(List<UserInRoleViewModel> users, string id)
        {
            _inRoleDetails.DefaultRoute = RequestDestination.ProfileAssignRoleToUsers;
            _inRoleDetails.Uri = _options.Value.CreateEditDefault;
            return await _inRoleDetails.AssignRemoveRoleToUsers(users, id);
        }

        public async Task<HttpResponseMessage> RemoveRoleFromUsers(List<UserInRoleViewModel> users, string id)
        {
            _inRoleDetails.DefaultRoute = RequestDestination.ProfileRemoveUsersFromRole;
            _inRoleDetails.Uri = _options.Value.CreateEditDefault;
            return await _inRoleDetails.AssignRemoveRoleToUsers(users, id);
        }
    }
}