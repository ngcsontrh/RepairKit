using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Entities;

namespace Data.Interfaces
{
    public interface IAddressUserRepository : IRepository<AddressUser>
    {
        Task<List<AddressUser>> GetByUserIdAsync(Guid userId);
        Task UpdateMainAddressAsync(Guid userId, Guid addressId);
    }
}
