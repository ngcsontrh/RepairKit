using Data.Config;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Data.Implementations
{
    public class AddressUserRepository : Repository<AddressUser>, IAddressUserRepository
    {
        public AddressUserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<AddressUser>> GetByUserIdAsync(Guid userId)
        {
            var entities = await _context.AddressUsers
                .Where(x => x.UserId == userId)
                .ToListAsync();
            return entities;
        }

        public async Task UpdateMainAddressAsync(Guid userId, Guid addressId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            await _context.AddressUsers
                .Where(x => x.UserId == userId && x.Id != addressId)
                .ExecuteUpdateAsync(setter => setter.SetProperty(x => x.AddressMain, false));

            await _context.AddressUsers
                .Where(x => x.UserId == userId && x.Id == addressId)
                .ExecuteUpdateAsync(setter => setter.SetProperty(x => x.AddressMain, true));

            await transaction.CommitAsync();

        }
    }
}
