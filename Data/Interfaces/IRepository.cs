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
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity, bool saveChanges = false);
        Task AddAsync(IEnumerable<T> entities, bool saveChanges = false);
        Task DeleteAsync(T entity, bool saveChanges = false);
        Task DeleteAsync(IEnumerable<T> entities, bool saveChanges = false);
        Task UpdateAsync(T entity, bool saveChanges = false);
        Task UpdateAsync(IEnumerable<T> entities, bool saveChanges = false);
    }
}
