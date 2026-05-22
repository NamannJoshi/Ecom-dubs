using EcomFinale.DataAccess.Entities;

namespace EcomFinale.DataAccess.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetById(int id);

    IQueryable<Order> GetAll();

    Task<Order> Create(Order order);

    Task SaveChangesAsync();
}