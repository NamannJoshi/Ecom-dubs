using EcomFinale.DataAccess.Entities;

namespace EcomFinale.DataAccess.Repositories;

public interface ICartRepository
{
    IQueryable<Cart> GetAllCarts(CartStatus? status = null);

    Task<Cart> Create(Cart cart);

    Task<Cart?> GetById(int id);

    Task<Cart?> GetByUserId(int userId, CartStatus? status = null);

    Task Delete(int id);

    Task SaveChanges();
}