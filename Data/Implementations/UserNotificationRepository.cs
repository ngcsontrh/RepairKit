using Data.Config;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Shared.Filters;

namespace Data.Implementations
{
    public class UserNotificationRepository : Repository<UserNotification>, IUserNotificationRepository
    {
        public UserNotificationRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<(List<UserNotification>, int)> GetNotificationPageByFilterAsync(Guid userId, UserNotificationFilter filter)
        {
            var query = _context.UserNotifications.AsQueryable();

            query = query.Where(x => x.UserId == userId);

            var entities = await query
                .Skip(filter.Offset)
                .Take(filter.Limit)
                .Include(x => x.Notification)
                .OrderBy(x => x.Id)
                .ToListAsync();
            var totalCount = await query.CountAsync();

            return (entities, totalCount);
        }

        public async Task MarkNotificationAsReadAsync(Guid userId, List<Guid> notificationIds)
        {
            await _context.UserNotifications
                .Where(x => x.UserId == userId && notificationIds.Contains(x.NotificationId))
                .ExecuteUpdateAsync(x => x.SetProperty(n => n.IsRead, true)
                                            .SetProperty(n => n.ReadAt, DateTime.UtcNow));
        }
    }
}
