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
                var uri = _options.Value.GetCategories;
                client.BaseAddress = new Uri(uri);
                var getCategoriesResponse = await client.GetAsync(RequestDestination.GetCategories);

                readResult = await getCategoriesResponse.Content.ReadAsAsync<IList<CategoryViewModel>>();
                client.Dispose();
            }
            return readResult;
        }

        public async Task<HttpResponseMessage> CreateCategory(CategoryCreateViewModel category)
        {
            HttpResponseMessage resultPostCategory = new();
            using (var client = new HttpClient())
            {
                var uri = _options.Value.PostEditProduct;
                client.BaseAddress = new Uri(uri);
                resultPostCategory = await client.PostAsJsonAsync(RequestDestination.PostCreateCategory, category);
            }
            return resultPostCategory;
        }

        public async Task<CategoryUpdateViewModel> EditCategory(Guid id)
        {
            CategoryUpdateViewModel getContent = null;
            using (var client = new HttpClient())
            {
                var uri = _options.Value.GetCategories;
                client.BaseAddress = new Uri(uri);
                var getCategoryResult = await client.GetAsync(RequestDestination.GetCategory + id);

                if (getCategoryResult.IsSuccessStatusCode)
                    getContent = await getCategoryResult.Content.ReadAsAsync<CategoryUpdateViewModel>();

            }
            return getContent;
        }
    }
}
