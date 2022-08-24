using MagazineManagment.BLL.ResponseService;

namespace MagazineManagment.BLL.RepositoryServices.GenericService
{
    public interface IGenericRepository<T, U, V> where T : class
    {
        Task<ResponseService<T>> Create(U obj);
        Task<ResponseService<T>> Update(U obj, V sc_obj);
    }
}