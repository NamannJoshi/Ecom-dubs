using EcomFinale.DataAccess.Repositories;
using EcomFinale.DataAccess.Repositories.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EcomFinale.DataAccess.Extensions;

public static class DataExtensions
{
    public static IServiceCollection AddDataExtensions(this IServiceCollection services, IConfiguration config)
    {
        return services
            .AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(config.GetConnectionString("AppDb"))
            );
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<ICartRepository, CartRepository>()
            .AddScoped<ICartItemRepository, CartItemRepository>()
            .AddScoped<IPaymentRepository, PaymentRepository>()
            .AddScoped<IProductCategoryRepository, ProductCategoryRepository>()
            .AddScoped<IProductRepository, ProductRepository>()
            .AddScoped<IRefreshTokenRepository, RefreshTokenRepository>()
            .AddScoped<IOrderRepository, OrderRepository>()
            .AddScoped<IOrderItemRepository, OrderItemRepository>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
