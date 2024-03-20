using Authentication.API.Common;
using System.Linq.Expressions;

namespace Authentication.API.DataAccess.Repositories.Common
{
    public interface IAsyncRepository<T> where T : EntityBase
    {
        Task<IReadOnlyList<T>> GetAllAsync(bool disableTracking = true);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true);
        Task<T> GetByIdAsync(Guid id, List<Expression<Func<T, object>>> includes = null);
        Task<T> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(T entity);
    }
}
