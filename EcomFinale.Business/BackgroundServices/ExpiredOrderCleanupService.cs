using EcomFinale.Business.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EcomFinale.Business.BackgroundServices;

public class ExpiredOrderCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory serviceProviderFactory;

    public ExpiredOrderCleanupService(IServiceScopeFactory  serviceProviderFactory)
    {
        this.serviceProviderFactory = serviceProviderFactory;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Logic to clean up expired orders goes here
            await CleanUpExpiredOrders();
            await Task.Delay(TimeSpan.FromSeconds(600), stoppingToken);
        }
    }

    private async Task CleanUpExpiredOrders()
    {
        // Implement the logic to identify and clean up expired orders
        var scope = this.serviceProviderFactory.CreateScope();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        // Example: Fetch orders that are pending for more than 30 minutes and mark them as abandoned
        await orderService.OrderRollback();
    }
}