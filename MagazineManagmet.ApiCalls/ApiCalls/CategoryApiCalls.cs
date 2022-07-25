using MagazineManagment.DTO.ViewModels;
using MagazineManagment.Shared.ApiUrlDestinations;
using MagazineManagment.Web.ApiCalls.ApiUrlValues;
using Microsoft.Extensions.Options;

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
            IEnumerable<CategoryViewModel> readResult = null;
            using (var client = new HttpClient())
            {
                var uri = _options.Value.CategoryGetOrDeleteDefaultUri;
                client.BaseAddress = new Uri(uri);
                var getCategoriesResponse = await client.GetAsync(RequestDestination.CategoryGetOrDeleteDefaultRoute);

                readResult = await getCategoriesResponse.Content.ReadAsAsync<IList<CategoryViewModel>>();
                client.Dispose();
            }
            return readResult;
        }

        public async Task<HttpResponseMessage> PostCreateCategory(CategoryCreateViewModel category)
        {
            HttpResponseMessage resultPostCategory = new();
            using (var client = new HttpClient())
            {
                var uri = _options.Value.CategoryCreateOrEditDefaultUri;
                client.BaseAddress = new Uri(uri);
                resultPostCategory = await client.PostAsJsonAsync(RequestDestination.CategoryCreateOrEditDefaultRoute, category);
                client.Dispose();
            }
            return resultPostCategory;
        }

        public async Task<CategoryUpdateViewModel> GetEditCategory(Guid id)
        {
            CategoryUpdateViewModel? getContent = null;
            using (var client = new HttpClient())
            {
                var uri = _options.Value.CategoryGetOrDeleteDefaultUri;
                client.BaseAddress = new Uri(uri);
                var getCategoryResult = await client.GetAsync(RequestDestination.CategoryGetOrDeleteDefaultRoute + "/" + id);
                getContent = await getCategoryResult.Content.ReadAsAsync<CategoryUpdateViewModel>();
                client.Dispose();
            }
            return getContent;
        }

        public async Task<HttpResponseMessage> PostEditCategory(CategoryUpdateViewModel category)
        {
            HttpResponseMessage? getPostEditResult = null;
            using (var client = new HttpClient())
            {
                var uri = _options.Value.CategoryCreateOrEditDefaultUri;
                client.BaseAddress = new Uri(uri);
                getPostEditResult = await client.PutAsJsonAsync(RequestDestination.CategoryCreateOrEditDefaultRoute, category);
                client.Dispose();
            }
            return getPostEditResult;
        }

        public async Task<HttpResponseMessage> PostDeleteCategory(Guid id)
        {
            HttpResponseMessage? deleteResult = null;
            using (var client = new HttpClient())
            {
                var uri = _options.Value.CategoryGetOrDeleteDefaultUri;
                client.BaseAddress = new Uri(uri);
                deleteResult = await client.DeleteAsync(RequestDestination.CategoryGetOrDeleteDefaultRoute + "/" + id);
                client.Dispose();
            }
            return deleteResult;
        }
    }
}