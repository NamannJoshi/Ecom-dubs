using EcomFinale.DataAccess.Entities;

namespace EcomFinale.DataAccess.Repositories.Implementation;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly AppDbContext context;

    public OrderItemRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<OrderItem> Create(OrderItem orderItem)
    {
        var createdOrderItem = await this.context.OrderItems.AddAsync(orderItem);
        await this.context.SaveChangesAsync();
        return createdOrderItem.Entity;
    }

    public async Task SaveChangesAsync()
    {
        await this.context.SaveChangesAsync();
    }
}