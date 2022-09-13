using MagazineManagment.DTO.ViewModels;
using MagazineManagment.Shared.ApiUrlDestinations;
using MagazineManagment.Web.ApiCalls.ApiUrlValues;
using MagazineManagmet.ApiCalls.ApiCall.GenericApiCall;
using MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace MagazineManagmet.ApiCalls.ApiCalls
{
    public class CategoryApiCalls : ICategoryApiCalls
    {
        private readonly IOptions<FetchApiValue> _config;
        private readonly IGenericApi<CategoryViewModel> _apiCall;
        private readonly IGenericApi<CategoryCreateViewModel> _PostMethodApi;
        private readonly IGenericApi<CategoryUpdateViewModel> _editMethodApi;

        public CategoryApiCalls(IOptions<FetchApiValue> config, IGenericApi<CategoryViewModel> apiCalls, IGenericApi<CategoryCreateViewModel> postMethodApi,
                                IGenericApi<CategoryUpdateViewModel> editMethodApi)
        {
            _config = config;
            _apiCall = apiCalls;
            _apiCall = apiCalls;
            _PostMethodApi = postMethodApi;
            _editMethodApi = editMethodApi;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategories(string token)

        {
            _apiCall.DefaultRoute = RequestDestination.CategoryGetOrDeleteDefaultRoute;
            _apiCall.Uri = _config.Value.GetDeleteDefault;
            _apiCall.Token = token;
            return await _apiCall.GetAllRecords(String.Empty);
        }

        public async Task<HttpResponseMessage> PostCreateCategory(CategoryCreateViewModel category, string token)
        {
            _PostMethodApi.DefaultRoute = RequestDestination.CategoryCreateOrEditDefaultRoute;
            _PostMethodApi.Uri = _config.Value.CreateEditDefault;
            _PostMethodApi.Token = token;
            return await _PostMethodApi.PostRecord(category);
        }

        public async Task<CategoryViewModel> GetEditCategory(Guid id, string token)
        {
            _apiCall.DefaultRoute = RequestDestination.CategoryGetOrDeleteDefaultRoute;
            _apiCall.Uri = _config.Value.GetDeleteDefault;
            _apiCall.Token = token;
            return await _apiCall.RecordDetails(id);
        }

        public async Task<HttpResponseMessage> PostEditCategory(CategoryUpdateViewModel category, string token)
        {
            _editMethodApi.DefaultRoute = RequestDestination.CategoryCreateOrEditDefaultRoute;
            _editMethodApi.Uri = _config.Value.CreateEditDefault;
            _editMethodApi.Token = token;
            return await _editMethodApi.Edit(category);

        }

        public async Task<HttpResponseMessage> PostDeleteCategory(Guid id, string token)
        {
            _apiCall.DefaultRoute = RequestDestination.CategoryGetOrDeleteDefaultRoute;
            _apiCall.Uri = _config.Value.GetDeleteDefault;
            _apiCall.Token = token;
            return await _apiCall.Delete(id);
        }

        public async Task<CategoryViewModel> ActivateCategory(Guid id, string token)
        {
            var client = new HttpClient();
            var uri = _config.Value.GetDeleteDefault;

            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var response = await client.GetAsync(RequestDestination.ActivateCategory + "/" + id);
            var readResponse = await response.Content.ReadAsAsync<CategoryViewModel>();

            client.Dispose();
            return readResponse;
        }

    }
}