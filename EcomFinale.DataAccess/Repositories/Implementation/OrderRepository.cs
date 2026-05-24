using EcomFinale.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcomFinale.DataAccess.Repositories.Implementation;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext context;

    public OrderRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Order> Create(Order order)
    {
        this.context.Orders.Add(order);
        await this.context.SaveChangesAsync();
        return order;
    }

    public IQueryable<Order> GetAll()
    {
        return this.context.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking();
    }

    public async Task<Order?> GetById(int id)
    {
        return await this.context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order?> GetByIdempotencyId(Guid idempotencyId)
    {
        return await this.context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.IdempotencyId == idempotencyId);
    }

    public async Task SaveChangesAsync()
    {
        await this.context.SaveChangesAsync();
    }
}