using Shared.Entities;
using Shared.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUserNotificationRepository : IRepository<UserNotification>
    {
        Task<(List<UserNotification>, int)> GetNotificationPageByFilterAsync(Guid userId, UserNotificationFilter filter);
        Task MarkNotificationAsReadAsync(Guid userId, List<Guid> ids);
    }
}
