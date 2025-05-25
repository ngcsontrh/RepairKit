using Data.Config;
using Data.Implementations;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared;

public static class Extensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, string connectionString)
    {        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.EnableSensitiveDataLogging();
        });

        // Register all repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAddressUserRepository, AddressUserRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICartDetailRepository, CartDetailRepository>();
        services.AddScoped<IDeviceDetailRepository, DeviceDetailRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
        services.AddScoped<IRepairmanFormRepository, RepairmanFormRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IServiceDetailRepository, ServiceDetailRepository>();
        services.AddScoped<IServiceDeviceRepository, ServiceDeviceRepository>();
        services.AddScoped<IUserNotificationRepository, UserNotificationRepository>();
        
        // Register UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static void SeedDefaultData(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (!context.Users.Any(u => u.Role == UserRole.Admin.ToString()))
        {
            context.Users.Add(new Shared.Entities.User
            {
                FullName = "Admin",
                Phone = "1234567890",
                Password = PasswordHelper.HashPassword("admin"),
                Role = UserRole.Admin.ToString(),
            });
            context.SaveChanges();
        }
    }

    public static void ApplyMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
}
