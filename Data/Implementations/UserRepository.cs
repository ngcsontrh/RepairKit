using Data.Config;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Entities;
using Shared.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByPhoneAsync(string phone)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Phone == phone);
            return user;
        }

        public async Task<List<User>> GetListWithFilterAsync(UserFilter filter)
        {
            var query = Filter(filter);
            var result = await query.ToListAsync();
            return result;
        }

        private IQueryable<User> Filter(UserFilter filter)
        {
            IQueryable<User> query = _context.Users;
            query = query.Where(x => x.Role != UserRole.Admin.ToString());
            if (!string.IsNullOrEmpty(filter.Role))
            {
                query = query.Where(x => x.Role == filter.Role);
            }
            if (!string.IsNullOrEmpty(filter.Status))
            {
                query = query.Where(x => x.Status == filter.Status);
            }
            if (filter.Offset.HasValue && filter.Limit.HasValue)
            {
                query = query
                    .Skip(filter.Offset.Value)
                    .Take(filter.Limit.Value);
            }
            return query;
        }
    }
}
