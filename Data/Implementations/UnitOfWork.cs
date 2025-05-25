using Data.Config;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IUserRepository UserRepository { get; }
        public IAddressUserRepository AddressUserRepository { get; }
        public ICartRepository CartRepository { get; }
        public ICartDetailRepository CartDetailRepository { get; }
        public IDeviceDetailRepository DeviceDetailRepository { get; }
        public INotificationRepository NotificationRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IOrderDetailRepository OrderDetailRepository { get; }
        public IRepairmanFormRepository RepairmanFormRepository { get; }
        public IServiceRepository ServiceRepository { get; }
        public IServiceDetailRepository ServiceDetailRepository { get; }
        public IServiceDeviceRepository ServiceDeviceRepository { get; }
        public IUserNotificationRepository UserNotificationRepository { get; }

        public UnitOfWork(
            AppDbContext context,
            IUserRepository userRepository,
            IAddressUserRepository addressUserRepository,
            ICartRepository cartRepository,
            ICartDetailRepository cartDetailRepository,
            IDeviceDetailRepository deviceDetailRepository,
            INotificationRepository notificationRepository,
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            IRepairmanFormRepository repairmanFormRepository,
            IServiceRepository serviceRepository,
            IServiceDetailRepository serviceDetailRepository,
            IServiceDeviceRepository serviceDeviceRepository,
            IUserNotificationRepository userNotificationRepository)
        {
            _context = context;
            UserRepository = userRepository;
            AddressUserRepository = addressUserRepository;
            CartRepository = cartRepository;
            CartDetailRepository = cartDetailRepository;
            DeviceDetailRepository = deviceDetailRepository;
            NotificationRepository = notificationRepository;
            OrderRepository = orderRepository;
            OrderDetailRepository = orderDetailRepository;
            RepairmanFormRepository = repairmanFormRepository;
            ServiceRepository = serviceRepository;
            ServiceDetailRepository = serviceDetailRepository;
            ServiceDeviceRepository = serviceDeviceRepository;
            UserNotificationRepository = userNotificationRepository;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
