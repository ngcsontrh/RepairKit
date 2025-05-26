using Data.Config;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity, bool saveChanges = false)
        {
            await _context.Set<T>().AddAsync(entity);
            if (saveChanges) await _context.SaveChangesAsync();
        }

        public async Task AddAsync(IEnumerable<T> entities, bool saveChanges = false)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            if (saveChanges) await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }

        public async Task DeleteAsync(T entity, bool saveChanges = false)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(IEnumerable<T> entities, bool saveChanges = false)
        {
            _context.Set<T>().RemoveRange(entities);
            if (saveChanges) await _context.SaveChangesAsync();
        }

        public async Task ExecuteDeleteAsync(Expression<Func<T, bool>> predicate)
        {
            await _context.Set<T>()
                .Where(predicate)
                .ExecuteDeleteAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<List<Guid>> GetIdsAsync(Expression<Func<T, bool>> predicate)
        {
            var ids = await _context.Set<T>()
                .Where(predicate)
                .Select(x => EF.Property<Guid>(x, "Id"))
                .ToListAsync();
            return ids;
        }

        public async Task<(List<T>, int)> GetPageAsync(int offset = 0, int limit = 10)
        {
            var entities = await _context.Set<T>()
                .Skip(offset)
                .Take(limit)
                .OrderBy(x => EF.Property<Guid>(x, "Id"))
                .ToListAsync();
            var totalCount = await _context.Set<T>().CountAsync();
            return (entities, totalCount);
        }

        public async Task UpdateAsync(T entity, bool saveChanges = false)
        {
            _context.Set<T>().Update(entity);
            if (saveChanges) await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(IEnumerable<T> entities, bool saveChanges = false)
        {
            _context.Set<T>().UpdateRange(entities);
            if (saveChanges) await _context.SaveChangesAsync();
        }
    }
}
