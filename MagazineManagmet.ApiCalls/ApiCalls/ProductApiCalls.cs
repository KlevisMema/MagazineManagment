using MagazineManagment.DTO.ViewModels;
using MagazineManagment.Shared.ApiUrlDestinations;
using MagazineManagment.Web.ApiCalls.ApiUrlValues;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace MagazineManagment.Web.ApiCalls
{
    public class ProductApiCalls : IProductApiCalls
    {

        private readonly IOptions<FetchApiValue> _config;

        public ProductApiCalls(IOptions<FetchApiValue> config)
        {
            _config = config;
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllProducts()
        {
            using HttpClient client = new();
            IEnumerable<ProductViewModel>? readResponse = null;
            var uri = _config.Value.ProductGet;
            client.BaseAddress = new Uri(uri);

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

                var response = await client.GetAsync(RequestDestination.GetCreateProductRoute);

                readResponse = await response.Content.ReadAsAsync<IList<CategoryNameOnlyViewModel>>();
                client.Dispose();
            }
            return readResponse;
        }

        public async Task<HttpResponseMessage> PostCreateProduct(ProductCreateViewModel product)
        {

            //string? BaseArrayImage = null;
            //using (MemoryStream ms = new())
            //{
            //    product.ImageFile.CopyTo(ms);
            //    var fileBytes = ms.ToArray();
            //    BaseArrayImage = Convert.ToBase64String(fileBytes);
            //}

            var BaseArrayImage = await ConvertImageToBase64(product.ImageFile);

            ProductCreateViewModelNoIFormFile newProduct = new()
            {
                ProductName = product.ProductName,
                SerialNumber = product.SerialNumber,
                Price = product.Price,
                ProductCategoryId = product.ProductCategoryId,
                Image = BaseArrayImage,
                CreatedBy = product.CreatedBy,
                ProductInStock = product.ProductInStock,
                CurrencyType = product.CurrencyType,
                ProductDescription = product.ProductDescription
            };


            using var client = new HttpClient();
            var uri = _config.Value.ProductPostCreateOrEditDefaultUri;
            client.BaseAddress = new Uri(uri);
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
            }

            using (var client = new HttpClient())
            {
                var uri = _config.Value.ProductPostCreateOrEditDefaultUri;
                client.BaseAddress = new Uri(uri);
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
            var getProduct = await client.GetAsync(RequestDestination.GetProductImage + id);

            var Task = await getProduct.Content.ReadAsAsync<ProductImageOnly>();
            return Task;
        }

        public static async Task<string> ConvertImageToBase64(IFormFile image)
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

    }
}