using Authentication.API.Common;
using Authentication.API.DataAccess.Contexts;
using Authentication.API.DataAccess.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Authentication.API.DataAccess.Repositories
{
    public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
    {
        protected readonly AuthenticationContext _dbContext;

        public RepositoryBase(AuthenticationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<int> DeleteAsync(T entity)
        {
            _dbContext.Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking)
                query = query.AsNoTracking();

            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking)
                query = query.AsNoTracking();

            if (includes != null)
            {
                foreach (var includeProperty in includes)
                    query = query.Include(includeProperty);
            }

            return await query.Where(predicate).ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id, List<Expression<Func<T, object>>> includes = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(_ => _.Id == id);
        }

        public async Task<int> UpdateAsync(T entity)
        {
            _dbContext.Update(entity);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
