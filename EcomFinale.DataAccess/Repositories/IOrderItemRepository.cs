using EcomFinale.DataAccess.Entities;

namespace EcomFinale.DataAccess.Repositories;

public interface IOrderItemRepository
{
    Task<OrderItem> Create(OrderItem orderItem);

    Task SaveChangesAsync();
}