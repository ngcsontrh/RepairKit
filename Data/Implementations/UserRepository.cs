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

        public async Task<User?> GetDetailAsync(Guid id)
        {
            var entity = await _context.Users
                .Include(x => x.Addresses)
                .FirstOrDefaultAsync(x => x.Id == id);
            return entity;
        }

        public async Task<List<User>> GetListWithFilterAsync(UserFilter filter)
        {
            var query = Filter(filter);
            var result = await query.ToListAsync();
            return result;
        }

        public async Task<(int today, int thisWeek, int thisMonth)> GetNewUserCountAsync()
        {
            var todayStart = DateTime.UtcNow.Date;
            var thisWeekStart = DateTime.UtcNow.Date.AddDays(-((7 + (int)DateTime.UtcNow.Date.DayOfWeek - 1) % 7));
            var thisMonthStart = new DateTime(DateTime.UtcNow.Date.Year, DateTime.UtcNow.Date.Month, 1);
            
            var todayEnd = todayStart.AddDays(1).AddTicks(-1);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddTicks(-1);
            var thisMonthEnd = new DateTime(DateTime.UtcNow.Date.Year, DateTime.UtcNow.Date.Month, 1).AddMonths(1).AddTicks(-1);

            var todayCount = await _context.Users.CountAsync(x => x.CreatedAt >= todayStart && x.CreatedAt <= todayEnd);
            var thisWeekCount = await _context.Users.CountAsync(x => x.CreatedAt >= thisWeekStart && x.CreatedAt <= thisWeekEnd);
            var thisMonthCount = _context.Users.CountAsync(x => x.CreatedAt >= thisMonthStart && x.CreatedAt <= thisMonthEnd);

            return (todayCount, thisWeekCount, await thisMonthCount);
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
            return query;
        }
    }
}
