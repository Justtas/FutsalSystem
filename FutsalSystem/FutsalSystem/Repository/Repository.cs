using FutsalSystem.Base.Interface;
using FutsalSystem.Repository.Interface;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FutsalSystem.Repository
{
    public class Repository : IRepository
    {
        private Context _context;

        public Repository(Context context)
        {
            _context = context;
        }

        public async Task DeleteAsync<T>(int id) where T : class, IBaseEntity
        {
            var entity = await QueryByIdAsync<T>(id);
            if (entity != null)
            {
                await Task.Run(() => _context.Set<T>().Remove(entity));
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsyncWithoutSave<T>(int id) where T : class, IBaseEntity
        {
            var entity = await QueryByIdAsync<T>(id);
            if (entity != null)
            {
                await Task.Run(() => _context.Set<T>().Remove(entity));
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<T> CreateAsync<T>(T data) where T : class, IBaseEntity
        {
            var result = _context.Set<T>().Add(data);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<T> CreateAsyncWithoutSave<T>(T data) where T : class, IBaseEntity
        {
            var result = _context.Set<T>().Add(data);
            return result.Entity;
        }

        public async Task<IQueryable<T>> QueryAsync<T>() where T : class, IBaseEntity
        {
            return await Task.Run(() => _context.Set<T>());
        }

        public async Task<T> QueryByIdAsync<T>(int id) where T : class, IBaseEntity
        {
            return await Task.Run(() => _context.Set<T>().SingleOrDefault(x => x.Id == id));
        }

        public async Task UpdateAsync<T>(int id, T data) where T : class, IBaseEntity
        {
            await Task.Run(() => _context.Set<T>().Update(data));
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsyncWithoutSave<T>(int id, T data) where T : class, IBaseEntity
        {
            await Task.Run(() => _context.Set<T>().Update(data));
        }
    }
}
