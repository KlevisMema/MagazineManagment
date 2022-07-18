using MagazineManagment.DTO.ViewModels;
using System.Web.Mvc;

namespace MagazineManagment.Web.ApiCalls
{
    public class ProductApiCalls
    {
        public static async Task<IEnumerable<ProductViewModel>> GetAllProducts(string Route)
        {
            IEnumerable<ProductViewModel> products = null;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7208/api/");

                var response = client.GetAsync(Route);
                response.Wait();

                HttpResponseMessage result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    var read = result.Content.ReadAsAsync<IList<ProductViewModel>>();
                    read.Wait();
                    products = read.Result.ToList();
                }
                return products;
            }
        }


        public static IEnumerable<CategoryNameOnlyViewModel> GetCreateProduct(string Route)
        {
            IEnumerable<CategoryNameOnlyViewModel> categories = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7208/api/Category/");

                var Response = client.GetAsync(Route);
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
    }
}


