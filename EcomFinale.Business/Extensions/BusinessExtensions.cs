using EcomFinale.Business.Services;
using EcomFinale.Business.Services.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace EcomFinale.Business.Extensions;

public static class BusinessExtensions
{
    public static IServiceCollection AddBusinessExtensions(this IServiceCollection services)
    {
        return services
            .AddScoped<IProductService, ProductService>()
            .AddScoped<IUserService, UserService>();
    }
}