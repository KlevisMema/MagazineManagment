using MagazineManagment.DTO.ViewModels;
using MagazineManagment.Shared.ApiUrlDestinations;
using MagazineManagment.Web.ApiCalls.ApiUrlValues;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections;
using System.Net.Http.Headers;
using System.Text;

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
            using (var ms = new MemoryStream())
            {
                product.ImageFile.CopyTo(ms);
                var fileBytes = ms.ToArray();
                BaseArrayImage = Convert.ToBase64String(fileBytes);
            }

            ProductCreateViewModelNoIFormFile newProduct = new ProductCreateViewModelNoIFormFile
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
            

            var uri = _config.Value.PostCreateProduct;
            var client = new HttpClient();
            client.BaseAddress = new Uri(uri);
            var result =  client.PostAsJsonAsync<ProductCreateViewModelNoIFormFile>(RequestDestination.PostCreateProductRoute, newProduct).Result;
            
            return result;
        }

        public ProductUpdateViewModel GetEdit(Guid id)
        {
            ProductUpdateViewModel editProduct = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7208/api/");

                var getTask = client.GetAsync($"Product/{id}");
                getTask.Wait();

                var getTaskResult = getTask.Result;
                if (getTaskResult.IsSuccessStatusCode)
                {
                    var readTask = getTaskResult.Content.ReadAsAsync<ProductUpdateViewModel>();
                    editProduct = readTask.Result;
                }
            }
            return editProduct;
        }

        public void PostEdit(ProductUpdateViewModel UpdateProduct)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7208/api/");
                var postTask = client.PutAsJsonAsync("Product", UpdateProduct);
                postTask.Wait();


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
    }
}


