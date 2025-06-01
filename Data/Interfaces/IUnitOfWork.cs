using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IAddressUserRepository AddressUserRepository { get; }
        ICartRepository CartRepository { get; }
        ICartDetailRepository CartDetailRepository { get; }
        IDeviceDetailRepository DeviceDetailRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        IRepairmanFormRepository RepairmanFormRepository { get; }
        IRepairmanFormDetailRepository RepairmanFormDetailRepository { get; }
        IServiceRepository ServiceRepository { get; }        
        IServiceDeviceRepository ServiceDeviceRepository { get; }
        IUserNotificationRepository UserNotificationRepository { get; }

        Task SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
