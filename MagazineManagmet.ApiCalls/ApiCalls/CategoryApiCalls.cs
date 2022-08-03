using MagazineManagment.DTO.ViewModels;
using MagazineManagment.Shared.ApiUrlDestinations;
using MagazineManagment.Web.ApiCalls.ApiUrlValues;
using MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace MagazineManagmet.ApiCalls.ApiCalls
{
    public class CategoryApiCalls : ICategoryApiCalls
    {
        private readonly IOptions<FetchApiValue> _options;
        private string GetIdentityUserName(HttpContext context)
        {
            var identityUser = context.User.Identity as ClaimsIdentity;
            var userName = identityUser.FindFirst(ClaimTypes.Name).Value;
            return userName;
        }

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

        public async Task<HttpResponseMessage> PostCreateCategory(CategoryCreateViewModel category, HttpContext context)
        {
            HttpResponseMessage resultPostCategory = new();
            using (var client = new HttpClient())
            {
                var uri = _options.Value.CategoryCreateOrEditDefaultUri;
                client.BaseAddress = new Uri(uri);
                category.CreatedBy = GetIdentityUserName(context);
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

        public async Task<HttpResponseMessage> PostEditCategory(CategoryUpdateViewModel category, HttpContext context)
        {
            HttpResponseMessage? getPostEditResult = null;
            using (var client = new HttpClient())
            {
                var uri = _options.Value.CategoryCreateOrEditDefaultUri;
                client.BaseAddress = new Uri(uri);
                category.UpdatedBy = GetIdentityUserName(context);
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