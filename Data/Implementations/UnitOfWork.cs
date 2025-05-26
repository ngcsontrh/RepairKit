using Data.Config;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
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
        private IDbContextTransaction? _transaction;

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
        public IRepairmanFormDetailRepository RepairmanFormDetailRepository { get; }

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
            IUserNotificationRepository userNotificationRepository,
            IRepairmanFormDetailRepository repairmanFormDetailRepository)
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
            RepairmanFormDetailRepository = repairmanFormDetailRepository;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Transaction has not been started.");
            }
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Transaction has not been started.");
            }
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}
