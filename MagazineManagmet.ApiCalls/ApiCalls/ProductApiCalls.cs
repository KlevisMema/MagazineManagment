using MagazineManagment.DTO.ViewModels;
using MagazineManagment.Shared.ApiUrlDestinations;
using MagazineManagment.Shared.Jwtbearer;
using MagazineManagment.Web.ApiCalls.ApiUrlValues;
using MagazineManagmet.ApiCalls.ApiCall.GenericApiCall;
using MagazineManagmet.ApiCalls.ApiCalls.ApiCallsInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace MagazineManagment.Web.ApiCalls
{
    public class ProductApiCalls : IProductApiCalls
    {
        private readonly IOptions<FetchApiValue> _config;
        private readonly IGenericApi<ProductViewModel> _apiCall;
        private readonly IGenericApi<ProductCreateViewModelNoIFormFile> _postMethodApi;
        private readonly IGenericApi<ProductUpdateViewModel> _editMethodApi;
        private readonly IGenericApi<ProductPostEditViewModel> _editMethodApi_;
        private readonly IGenericApi<ProductImageOnly> _getProductImage;
        private readonly IGenericApi<ProductsRecordCopyViewModel> _GetEmployeeWorkApiCall;

        public ProductApiCalls (IOptions<FetchApiValue> config, IGenericApi<ProductViewModel> apiCall, IGenericApi<ProductCreateViewModelNoIFormFile> postMethodApi,
                               IGenericApi<ProductUpdateViewModel> editMethodApi, IGenericApi<ProductPostEditViewModel> editMethodApi_,
                               IGenericApi<ProductImageOnly> getProductImage, IGenericApi<ProductsRecordCopyViewModel> GetEmployeeWorkApiCal)
        {
            _config = config;
            _apiCall = apiCall;
            _postMethodApi = postMethodApi;
            _editMethodApi = editMethodApi;
            _editMethodApi_ = editMethodApi_;
            _getProductImage = getProductImage;
            _GetEmployeeWorkApiCall = GetEmployeeWorkApiCal;    
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
            _apiCall.Uri = _config.Value.GetDeleteDefault;
            _apiCall.DefaultRoute = RequestDestination.ProductGetOrDeleteDefaultRoute;
            return await _apiCall.GetAllRecords(String.Empty);
        }

        public async Task<IEnumerable<CategoryNameOnlyViewModel>> GetCreateProduct()
        {
            var client = new HttpClient();

            var uri = _config.Value.GetAllCategories;

            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", TokenHolder.Token);

            var response = await client.GetAsync(RequestDestination.GetCreateProductRoute);
            var readResponse = await response.Content.ReadAsAsync<IList<CategoryNameOnlyViewModel>>();

            client.Dispose();
            return readResponse;
        }

        public async Task<HttpResponseMessage> PostCreateProduct(ProductCreateViewModel product)
        {

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

            _postMethodApi.DefaultRoute = RequestDestination.ProductCreateOrEditDefaultRoute;
            _postMethodApi.Uri = _config.Value.CreateEditDefault;
            return await _postMethodApi.PostRecord(newProduct);
        }

        public async Task<ProductUpdateViewModel> GetEditProduct(Guid id)
        {
            _editMethodApi.DefaultRoute = RequestDestination.ProductGetOrDeleteDefaultRoute;
            _editMethodApi.Uri = _config.Value.GetDeleteDefault;
            return await _editMethodApi.RecordDetails(id);
        }

        public async Task<HttpResponseMessage> PostEditProduct(ProductUpdateViewModel product)
        {
            ProductPostEditViewModel newUpdatedProduct = new();

            //case when user has not changed the image in the edit form
            if (product.ImageFile == null)
            {
                var getProduct = await GetProductImage(product.Id);

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
                newUpdatedProduct.ProductCategoryId = product.ProductCategoryId;
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
                newUpdatedProduct.ProductCategoryId = product.ProductCategoryId;
            }

            _editMethodApi_.DefaultRoute = RequestDestination.ProductCreateOrEditDefaultRoute;
            _editMethodApi_.Uri = _config.Value.CreateEditDefault;
            return await _editMethodApi_.Edit(newUpdatedProduct);
        }

        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            _apiCall.Uri = _config.Value.GetDeleteDefault;
            _apiCall.DefaultRoute = RequestDestination.ProductGetOrDeleteDefaultRoute;
            return await _apiCall.Delete(id);
        }

        public async Task<ProductImageOnly> GetProductImage(Guid id)
        {
            _getProductImage.Uri = _config.Value.GetDeleteDefault;
            _getProductImage.DefaultRoute = RequestDestination.GetProductImage;
            return await _getProductImage.RecordDetails(id);
        }

        public async Task<IEnumerable<ProductsRecordCopyViewModel>> GetProductChangesByEmpolyees()
        {
            _GetEmployeeWorkApiCall.Uri = _config.Value.GetDeleteDefault;
            _GetEmployeeWorkApiCall.DefaultRoute = RequestDestination.ProductChangesMadeByEmployee;
            return await _GetEmployeeWorkApiCall.GetAllRecords(String.Empty);
        }

        public async Task<HttpResponseMessage> DeleteProductChangeByEmployee(Guid id)
        {
            _apiCall.Uri = _config.Value.GetDeleteDefault;
            _apiCall.DefaultRoute = RequestDestination.ProductChangesMadeByEmployeeDeleteRoute;
            return await _apiCall.Delete(id);
        }

        public async Task<ProductViewModel> DetailsOfProductChangedByEmployee(Guid id)
        {
            _apiCall.Uri = _config.Value.GetDeleteDefault;
            _apiCall.DefaultRoute = RequestDestination.ProductGetOrDeleteDefaultRoute;
            return await _apiCall.RecordDetails(id);
        }

        public async Task<IEnumerable<ProductViewModel>> SearchProduct(string productName)
        {
            if (string.IsNullOrEmpty(productName))
                return await GetAllProducts();

            _apiCall.Uri = _config.Value.GetDeleteDefault;
            _apiCall.DefaultRoute = RequestDestination.SearchProduct;
            return await _apiCall.GetAllRecords(productName);
        }
    }
}