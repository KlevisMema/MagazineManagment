using MagazineManagment.Shared.Jwtbearer;
using System.Net.Http.Headers;

namespace MagazineManagmet.ApiCalls.ApiCall.GenericApiCall
{
    public class GenericApi<T> : IGenericApi<T> where T : class
    {
        public string Uri { get; set; }
        public string DefaultRoute { get; set; }
        public string Token { get; set; } 
        private HttpClient Client { get; set; } = new HttpClient();

        public GenericApi()
        {
        }

        public GenericApi(string Uri, string DefaultRoute, string token)
        {
            this.Uri = Uri;
            this.DefaultRoute = DefaultRoute;
            Token = token;
        }

        // Get all records api call
        public async Task<IEnumerable<T>> GetAllRecords(object productName)
        {
            Client.BaseAddress = new Uri(Uri);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token);

            var getResponse = await Client.GetAsync(DefaultRoute + productName);
            var readResult = await getResponse.Content.ReadAsAsync<IList<T>>();

            Client.Dispose();

            return readResult;
        }

        // Post record api call
        public async Task<HttpResponseMessage> PostRecord(T obj)
        {
            Client.BaseAddress = new Uri(Uri);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token);

            var result = await Client.PostAsJsonAsync(DefaultRoute, obj);
            Client.Dispose();

            return result;
        }

        // Get a single record by id api call
        public async Task<T> RecordDetails(object id)
        {
            Client.BaseAddress = new Uri(Uri);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token);

            var getTask = await Client.GetAsync(DefaultRoute + "/" + id);
            var readTask = await getTask.Content.ReadAsAsync<T>();

            Client.Dispose();
            return readTask;
        }

        // Post edited record api call
        public async Task<HttpResponseMessage> Edit(T obj)
        {
            Client.BaseAddress = new Uri(Uri);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token);

            var getPostEditResult = await Client.PutAsJsonAsync(DefaultRoute, obj);

            Client.Dispose();
            return getPostEditResult;
        }

        // Delete product api call
        public async Task<HttpResponseMessage> Delete(object id)
        {
            Client.BaseAddress = new Uri(Uri);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token);

            var deleteResult = await Client.DeleteAsync(DefaultRoute + "/" + id);

            Client.Dispose();
            return deleteResult;
        }

        // Get a list of records by some  id  
        public async Task<IEnumerable<T>> GetRecordsDetailsById(object id)
        {
            IEnumerable<T>? readResult = null;

            Client.BaseAddress = new Uri(Uri);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token);

            var getResponse = await Client.GetAsync(DefaultRoute + id);

            if (getResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                return readResult;

            readResult = await getResponse.Content.ReadAsAsync<IEnumerable<T>>();

            Client.Dispose();
            return readResult;
        }

        // AssignRole / Remove  role
        public async Task<HttpResponseMessage> AssignRemoveRoleToUsers(List<T> users, object id)
        {

            Client.BaseAddress = new Uri(Uri);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token);

            var resultPostRoleToUsers = await Client.PostAsJsonAsync(DefaultRoute + id, users);

            Client.Dispose();
            return resultPostRoleToUsers;
        }
    }
}