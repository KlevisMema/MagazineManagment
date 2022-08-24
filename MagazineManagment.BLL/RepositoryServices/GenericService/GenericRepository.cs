using AutoMapper;
using MagazineManagment.BLL.ResponseService;
using MagazineManagment.DAL.DataContext;
using Microsoft.EntityFrameworkCore;

namespace MagazineManagment.BLL.RepositoryServices.GenericService
{
    public class GenericRepository<T, U, V> : IGenericRepository<T, U, V> where T : class
                                                                          where U : class
                                                                          where V : class
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        private readonly DbSet<U> _table = null;


        public GenericRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _table = _applicationDbContext.Set<U>();
        }

        public GenericRepository()
        {
        }


        public async Task<ResponseService<T>> Create(U obj)
        {
            try
            {
                await _applicationDbContext.AddAsync(obj);
                await _applicationDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ResponseService<T>.ExceptioThrow(ex.Message);
            }
            return ResponseService<T>.Ok(_mapper.Map<T>(obj));
        }

        public async Task<ResponseService<T>> Update(U obj,V sc_obj)
        {
            try
            {
                _applicationDbContext.Entry(sc_obj).CurrentValues.SetValues(obj);
                await _applicationDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ResponseService<T>.ExceptioThrow(ex.Message);
            }
            return ResponseService<T>.Ok(_mapper.Map<T>(obj));
        }
    }
}