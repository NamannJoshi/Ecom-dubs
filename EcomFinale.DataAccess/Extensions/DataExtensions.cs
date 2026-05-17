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
                   .UseSnakeCaseNamingConvention()
        );
    }
}
