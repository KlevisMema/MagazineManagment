using MagazineManagment.DTO.ViewModels;
using MagazineManagment.Shared.ApiUrlDestinations;
using MagazineManagment.Shared.Jwtbearer;
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

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategories()
        {
            _apiCall.DefaultRoute = RequestDestination.CategoryGetOrDeleteDefaultRoute;
            _apiCall.Uri = _config.Value.CategoryGetOrDeleteDefaultUri;
            _apiCall.Token = TokenHolder.Token;
            return await _apiCall.GetAllRecords(String.Empty);
        }

        public async Task<HttpResponseMessage> PostCreateCategory(CategoryCreateViewModel category)
        {
            _PostMethodApi.DefaultRoute = RequestDestination.CategoryCreateOrEditDefaultRoute;
            _PostMethodApi.Uri = _config.Value.CategoryCreateOrEditDefaultUri;
            _PostMethodApi.Token = TokenHolder.Token;
            return await _PostMethodApi.PostRecord(category);
        }

        public async Task<CategoryViewModel> GetEditCategory(Guid id)
        {
            _apiCall.DefaultRoute = RequestDestination.CategoryGetOrDeleteDefaultRoute;
            _apiCall.Uri = _config.Value.CategoryGetOrDeleteDefaultUri;
            _apiCall.Token = TokenHolder.Token;
            return await _apiCall.RecordDetails(id);
        }

        public async Task<HttpResponseMessage> PostEditCategory(CategoryUpdateViewModel category)
        {
            _editMethodApi.DefaultRoute = RequestDestination.CategoryCreateOrEditDefaultRoute;
            _editMethodApi.Token = TokenHolder.Token;
            _editMethodApi.Uri = _config.Value.CategoryCreateOrEditDefaultUri;
            return await  _editMethodApi.Edit(category);

        }

        public async Task<HttpResponseMessage> PostDeleteCategory(Guid id)
        {
            _apiCall.DefaultRoute = RequestDestination.CategoryGetOrDeleteDefaultRoute;
            _apiCall.Uri = _config.Value.CategoryGetOrDeleteDefaultUri;
            _apiCall.Token = TokenHolder.Token;
            return await _apiCall.Delete(id);
        }
    }
}