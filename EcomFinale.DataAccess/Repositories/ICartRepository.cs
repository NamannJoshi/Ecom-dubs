using EcomFinale.DataAccess.Entities;

namespace EcomFinale.DataAccess.Repositories;

public interface ICartRepository
{
    IQueryable<Cart> GetAllCarts();

    Task<Cart> Create(Cart cart);

    Task<Cart?> GetById(int id);

    Task Delete(int id);

    Task SaveChanges();
}