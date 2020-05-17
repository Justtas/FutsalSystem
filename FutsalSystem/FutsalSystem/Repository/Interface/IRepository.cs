using System.Linq;
using System.Threading.Tasks;
using FutsalSystem.Base.Interface;

namespace FutsalSystem.Repository.Interface
{
    public interface IRepository
    {
        Task<T> CreateAsync<T>(T data) where T : class, IBaseEntity;
        Task<IQueryable<T>> QueryAsync<T>() where T : class, IBaseEntity;
        Task<T> QueryByIdAsync<T>(int id) where T : class, IBaseEntity;
        Task UpdateAsync<T>(int id, T data) where T : class, IBaseEntity;
        Task DeleteAsync<T>(int id) where T : class, IBaseEntity;
        Task DeleteAsyncWithoutSave<T>(int id) where T : class, IBaseEntity;
        Task<T> CreateAsyncWithoutSave<T>(T data) where T : class, IBaseEntity;
        Task UpdateAsyncWithoutSave<T>(int id, T data) where T : class, IBaseEntity;
        Task SaveChangesAsync();
    }
}
