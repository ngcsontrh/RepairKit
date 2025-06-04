using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity, bool saveChanges = false);
        Task AddAsync(IEnumerable<T> entities, bool saveChanges = false);
        Task DeleteAsync(T entity, bool saveChanges = false);
        Task DeleteAsync(IEnumerable<T> entities, bool saveChanges = false);
        Task UpdateAsync(T entity, bool saveChanges = false);
        Task UpdateAsync(IEnumerable<T> entities, bool saveChanges = false);
        Task<(List<T>, int)> GetPageAsync(int offset = 0, int limit = 10, Expression<Func<T, bool>>? predicate = null);
        Task<List<Guid>> GetIdsAsync(Expression<Func<T, bool>> predicate);
        Task ExecuteDeleteAsync(Expression<Func<T, bool>> predicate);
        Task SaveChangesAsync();
        Task<int> GetCountAsync(Expression<Func<T, bool>>? predicate = null);
    }
}
