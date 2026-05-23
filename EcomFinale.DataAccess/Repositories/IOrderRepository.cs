using EcomFinale.DataAccess.Entities;

namespace EcomFinale.DataAccess.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetById(int id);

    Task<Order?> GetByIdempotencyId(Guid idempotencyId);

    IQueryable<Order> GetAll();

    Task<Order> Create(Order order);

    Task SaveChangesAsync();
}