namespace MagazineManagmet.ApiCalls.ApiCall.GenericApiCall
{
    public interface IGenericApi<T>
        where T : class
    {
        string DefaultRoute { get; set; }
        string Uri { get; set; }

        Task<IEnumerable<T>> GetAllRecords(object productName);
        Task<HttpResponseMessage> PostRecord(T obj);
        Task<T> RecordDetails(object id);
        Task<HttpResponseMessage> Edit(T obj);
        Task<HttpResponseMessage> Delete(object id);
        Task<IEnumerable<T>> GetRecordsDetailsById(object id);
        Task<HttpResponseMessage> AssignRemoveRoleToUsers(List<T> users, object id);
    }
}