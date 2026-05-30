using EcomFinale.DataAccess.Options;

namespace EcomFinale.Web.Extensions;

public static class StripeExtension
{
    public static IServiceCollection AddStripeExtension(this IServiceCollection services, IConfiguration config)
    {
        return services.Configure<StripeOptions>(config.GetSection("Stripe"));
    }
}