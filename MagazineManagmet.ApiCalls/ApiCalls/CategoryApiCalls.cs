using MagazineManagment.DTO.ViewModels;
using MagazineManagment.Shared.ApiUrlDestinations;
using MagazineManagment.Shared.Jwtbearer;
using MagazineManagment.Web.ApiCalls.ApiUrlValues;
using MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace MagazineManagmet.ApiCalls.ApiCalls
{
    public class CategoryApiCalls : ICategoryApiCalls
    {
        private readonly IOptions<FetchApiValue> _options;

        public CategoryApiCalls(IOptions<FetchApiValue> options)
        {
            _options = options;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategories()
        {
            var client = new HttpClient();
            var uri = _options.Value.CategoryGetOrDeleteDefaultUri;

            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);

            var getCategoriesResponse = await client.GetAsync(RequestDestination.CategoryGetOrDeleteDefaultRoute);
            var readResult = await getCategoriesResponse.Content.ReadAsAsync<IList<CategoryViewModel>>();

            client.Dispose();
            
            return readResult;
        }

        public async Task<HttpResponseMessage> PostCreateCategory(CategoryCreateViewModel category)
        {
            var client = new HttpClient();
            var uri = _options.Value.CategoryCreateOrEditDefaultUri;

            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);

            var resultPostCategory = await client.PostAsJsonAsync(RequestDestination.CategoryCreateOrEditDefaultRoute, category);
            client.Dispose();

            return resultPostCategory;
        }

        public async Task<CategoryViewModel> GetEditCategory(Guid id)
        {
            var client = new HttpClient();
            var uri = _options.Value.CategoryGetOrDeleteDefaultUri;

            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);

            var getCategoryResult = await client.GetAsync(RequestDestination.CategoryGetOrDeleteDefaultRoute + "/" + id);
            var getContent = await getCategoryResult.Content.ReadAsAsync<CategoryViewModel>();

            client.Dispose();
            return getContent;
        }

        public async Task<HttpResponseMessage> PostEditCategory(CategoryUpdateViewModel category)
        {
            var client = new HttpClient();
            var uri = _options.Value.CategoryCreateOrEditDefaultUri;

            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);

            var getPostEditResult = await client.PutAsJsonAsync(RequestDestination.CategoryCreateOrEditDefaultRoute, category);

            client.Dispose();
            return getPostEditResult;
        }

        public async Task<HttpResponseMessage> PostDeleteCategory(Guid id)
        {
            var client = new HttpClient();
            var uri = _options.Value.CategoryGetOrDeleteDefaultUri;

            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);

            var deleteResult = await client.DeleteAsync(RequestDestination.CategoryGetOrDeleteDefaultRoute + "/" + id);

            client.Dispose();
            return deleteResult;
        }
    }
}