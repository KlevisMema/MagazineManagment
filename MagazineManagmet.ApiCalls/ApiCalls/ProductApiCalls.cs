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

        public IEnumerable<ProductViewModel> GetAllProducts()
        {
            var uri = _config.Value.GetProducts;

            IEnumerable<ProductViewModel> products = null;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(uri);

                var response = client.GetAsync(RequestDestination.Product);
                response.Wait();

                HttpResponseMessage result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    var read = result.Content.ReadAsAsync<IList<ProductViewModel>>();
                    read.Wait();
                    products = read.Result;
                }
                return products;
            }
        }

        public IEnumerable<CategoryNameOnlyViewModel> GetCreateProduct()
        {
            var uri = _config.Value.GetCreateProduct;

            IEnumerable<CategoryNameOnlyViewModel> categories = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(uri);

                var Response = client.GetAsync(RequestDestination.GetCreateProductRoute);
                Response.Wait();

                var result = Response.Result;

                if (result.IsSuccessStatusCode)
                {
                    var read = result.Content.ReadAsAsync<IList<CategoryNameOnlyViewModel>>();
                    read.Wait();
                    categories = read.Result;
                }
                else
                {
                    categories = Enumerable.Empty<CategoryNameOnlyViewModel>().ToList();
                }
            }

            return categories;
        }

        public async Task<HttpResponseMessage> PostCreateProduct(ProductCreateViewModel product)
        {

            string BaseArrayImage = null;
            using (MemoryStream ms = new MemoryStream())
            {
                product.ImageFile.CopyTo(ms);
                var fileBytes = ms.ToArray();
                BaseArrayImage = Convert.ToBase64String(fileBytes);
            }

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


            using (var client = new HttpClient())
            {
                var uri = _config.Value.PostCreateProduct;
                client.BaseAddress = new Uri(uri);
                var result = await client.PostAsJsonAsync(RequestDestination.PostCreateProductRoute, newProduct);
                return result;
            }

                
            
            //var content = await result.Content.ReadAsStringAsync();
            //var contentSplitted = content.Split(@"""").ToList();
            //var errorMessageIndex = contentSplitted.IndexOf("reasonPhrase") + 2;
            //var ErrorMessage = contentSplitted[errorMessageIndex];

            //if (contentSplitted.Contains("OK") == true)
            //{
            //    return new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
            //}

            //return new HttpResponseMessage
            //{
            //    ReasonPhrase = ErrorMessage,
            //    StatusCode = System.Net.HttpStatusCode.BadRequest,
            //};
        }

        public async Task<ProductUpdateViewModel> GetEditProduct(Guid id)
        {
            ProductUpdateViewModel editProduct = null;

            using (var client = new HttpClient())
            {
                var uri = _config.Value.GetProducts;
                client.BaseAddress = new Uri(uri);

                var getTask = await client.GetAsync(RequestDestination.GetEditProduct + id);


                if (getTask.IsSuccessStatusCode)
                {
                    var readTask = await getTask.Content.ReadAsAsync<ProductUpdateViewModel>();
                    editProduct = readTask;
                }
            }
            return editProduct;
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
                var uri = _config.Value.PostEditProduct;
                client.BaseAddress = new Uri(uri);
                var postTask = await client.PutAsJsonAsync(RequestDestination.PostEditProduct, newUpdatedProduct);
                return postTask;
            }
        }

        public void Delete(Guid id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7208/api/");
                var deleteTask = client.DeleteAsync($"Product/{id}");
                deleteTask.Wait();
            }
        }

        public async Task<ProductImageOnly> GetProductImage(Guid id)
        {
            ProductImageOnly productImageOnly = null;
            using (var client = new HttpClient())
            {
                var uri = _config.Value.GetProductImage;
                client.BaseAddress = new Uri(uri);
                var getProduct = await client.GetAsync(RequestDestination.GetProductImage + id);

                if (getProduct.IsSuccessStatusCode)
                {
                    var Task = await getProduct.Content.ReadAsAsync<ProductImageOnly>();
                    productImageOnly = Task;
                }
                return productImageOnly;
            }
        }

        public static async Task<string> ConvertImageToBase64(IFormFile image)
        {
            string BaseArrayImage = null;

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