using MagazineManagment.DTO.ViewModels;
using MagazineManagment.Web.ApiCalls.ApiUrlValues;
using MagazineManagmet.ApiCalls.ApiCalls.ApiUrlValues;
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

                var response = client.GetAsync(UrlDestination.Product);
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

                var Response = client.GetAsync(UrlDestination.GetCreateProductRoute);
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

        //public async Task<ProductViewModel> PostCreateProduct(ProductCreateViewModel product)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("https://localhost:7208/api/Product");

        //        // Post 
        //        var postTask = client.PostAsJsonAsync<ProductCreateViewModel>("Product", product);
        //        postTask.Wait();

        //        var result = postTask.Result;

        //        if (result.IsSuccessStatusCode)
        //        {
                   
        //        }
        //    }

         

        //}
    }
} 


