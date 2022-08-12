using MagazineManagment.DTO.ViewModels;
using MagazineManagment.Shared.ApiUrlDestinations;
using MagazineManagment.Shared.Jwtbearer;
using MagazineManagment.Web.ApiCalls.ApiUrlValues;
using MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace MagazineManagment.Web.ApiCalls
{
    public class ProductApiCalls : IProductApiCalls
    {
        private readonly IOptions<FetchApiValue> _config;
        public ProductApiCalls(IOptions<FetchApiValue> config)
        {
            _config = config;
        }

        private string GetIdentityUserName(HttpContext context)
        {
            var identityUser = context.User.Identity as ClaimsIdentity;
            var userName = identityUser.FindFirst(ClaimTypes.Name).Value;
            return userName;
        }

        private static async Task<string> ConvertImageToBase64(IFormFile image)
        {
            string? BaseArrayImage = null;

            using (MemoryStream ms = new())
            {
                await image.CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                BaseArrayImage = Convert.ToBase64String(fileBytes);
            }
            return BaseArrayImage;
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllProducts()
        {
            using HttpClient client = new();
            IEnumerable<ProductViewModel>? readResponse = null;
            var uri = _config.Value.ProductGet;
            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);
            var response = await client.GetAsync(RequestDestination.ProductGetOrDeleteDefaultRoute);
            readResponse = await response.Content.ReadAsAsync<IList<ProductViewModel>>();
            
            client.Dispose();
            return readResponse;
        }

        public async Task<IEnumerable<CategoryNameOnlyViewModel>> GetCreateProduct()
        {

            IEnumerable<CategoryNameOnlyViewModel>? readResponse = null;

            using (var client = new HttpClient())
            {
                var uri = _config.Value.GetAllCategories;
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);
                var response = await client.GetAsync(RequestDestination.GetCreateProductRoute);

                readResponse = await response.Content.ReadAsAsync<IList<CategoryNameOnlyViewModel>>();
                client.Dispose();
            }
            return readResponse;
        }

        public async Task<HttpResponseMessage> PostCreateProduct(ProductCreateViewModel product,HttpContext context)
        {

            var BaseArrayImage = await ConvertImageToBase64(product.ImageFile);

            ProductCreateViewModelNoIFormFile newProduct = new()
            {
                ProductName = product.ProductName,
                SerialNumber = product.SerialNumber,
                Price = product.Price,
                ProductCategoryId = product.ProductCategoryId,
                Image = BaseArrayImage,
                CreatedBy = GetIdentityUserName(context),
                ProductInStock = product.ProductInStock,
                CurrencyType = product.CurrencyType,
                ProductDescription = product.ProductDescription
            };


            using var client = new HttpClient();
            var uri = _config.Value.ProductPostCreateOrEditDefaultUri;
            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);
            var result = await client.PostAsJsonAsync(RequestDestination.ProductCreateOrEditDefaultRoute, newProduct);
            client.Dispose();
            return result;
        }

        public async Task<ProductUpdateViewModel> GetEditProduct(Guid id)
        {
            ProductUpdateViewModel? readTask = null;

            using (var client = new HttpClient())
            {
                var uri = _config.Value.ProductGet;
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);
                var getTask = await client.GetAsync(RequestDestination.ProductGetOrDeleteDefaultRoute + "/" + id);
                readTask = await getTask.Content.ReadAsAsync<ProductUpdateViewModel>();
                client.Dispose();
            }
            return readTask;
        }

        public async Task<HttpResponseMessage> PostEditProduct(ProductUpdateViewModel product)
        {
            ProductPostEditViewModel newUpdatedProduct = new();
            
            //case when user has not changed the image in the edit form
            if (product.ImageFile == null)
            {
                ProductImageOnly getProduct = await GetProductImage(product.Id);
                if (getProduct == null)
                {
                    return new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.NotFound };
                }
                newUpdatedProduct.Id = product.Id;
                newUpdatedProduct.ProductName = product.ProductName;
                newUpdatedProduct.SerialNumber = product.SerialNumber;
                newUpdatedProduct.Price = product.Price;
                newUpdatedProduct.Image = getProduct.Image;
                newUpdatedProduct.CreatedBy = product.CreatedBy;
                newUpdatedProduct.ProductInStock = product.ProductInStock;
                newUpdatedProduct.CurrencyType = product.CurrencyType;
                newUpdatedProduct.ProductDescription = product.ProductDescription;
                newUpdatedProduct.UserName = product.UserName;
            }
            else
            {
                var imageToBase64 = await ConvertImageToBase64(product.ImageFile);
                newUpdatedProduct.Id = product.Id;
                newUpdatedProduct.ProductName = product.ProductName;
                newUpdatedProduct.SerialNumber = product.SerialNumber;
                newUpdatedProduct.Price = product.Price;
                newUpdatedProduct.Image = imageToBase64;
                newUpdatedProduct.CreatedBy = product.CreatedBy;
                newUpdatedProduct.ProductInStock = product.ProductInStock;
                newUpdatedProduct.CurrencyType = product.CurrencyType;
                newUpdatedProduct.ProductDescription = product.ProductDescription;
                newUpdatedProduct.UserName = product.UserName;
            }

            using (var client = new HttpClient())
            {
                var uri = _config.Value.ProductPostCreateOrEditDefaultUri;
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);
                var postTask = await client.PutAsJsonAsync(RequestDestination.ProductCreateOrEditDefaultRoute, newUpdatedProduct);
                return postTask;
            }
        }

        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            HttpResponseMessage? deleteResult = null;
            using (var client = new HttpClient())
            {
                var uri = _config.Value.ProductGet;
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);
                deleteResult = await client.DeleteAsync(RequestDestination.ProductGetOrDeleteDefaultRoute + "/" + id);
                client.Dispose();
            }
            return deleteResult;
        }

        public async Task<ProductImageOnly> GetProductImage(Guid id)
        {

            using var client = new HttpClient();
            var uri = _config.Value.ProductGet;
            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);
            var getProduct = await client.GetAsync(RequestDestination.GetProductImage + id);

            var Task = await getProduct.Content.ReadAsAsync<ProductImageOnly>();
            return Task;
        }

        public async Task<IEnumerable<ProductsRecordCopyViewModel>> GetProductChangesByEmpolyees()
        {
            using HttpClient client = new();
            IEnumerable<ProductsRecordCopyViewModel>? readResponse = null;
            var uri = _config.Value.ProductGet;
            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);
            var response = await client.GetAsync(RequestDestination.ProductChangesMadeByEmployee);
            readResponse = await response.Content.ReadAsAsync<IList<ProductsRecordCopyViewModel>>();
            client.Dispose();
            return readResponse;
        }

        public async Task<HttpResponseMessage> DeleteProductChangeByEmployee(Guid id)
        {
            HttpResponseMessage? deleteResult = null;
            using (var client = new HttpClient())
            {
                var uri = _config.Value.ProductGet;
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);
                deleteResult = await client.DeleteAsync(RequestDestination.ProductChangesMadeByEmployeeDeleteRoute + id);
                client.Dispose();
            }
            return deleteResult;
        }

        public async Task<ProductViewModel> DetailsOfProductChangedByEmployee(Guid id)
        {
            ProductViewModel? readTask = null;

            using (var client = new HttpClient())
            {
                var uri = _config.Value.ProductGet;
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);
                var getTask = await client.GetAsync(RequestDestination.ProductGetOrDeleteDefaultRoute + "/" + id);
                readTask = await getTask.Content.ReadAsAsync<ProductViewModel>();
                client.Dispose();
            }
            return readTask;
        }
    }
}