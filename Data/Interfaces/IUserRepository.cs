using Shared;
using Shared.Entities;
using Shared.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<List<User>> GetListWithFilterAsync(UserFilter filter);
        public Task<User?> GetByPhoneAsync(string phone);
        public Task<User?> GetDetailAsync(Guid id);
        public Task<(int today, int thisWeek, int thisMonth)> GetNewUserCountAsync();
    }
}
