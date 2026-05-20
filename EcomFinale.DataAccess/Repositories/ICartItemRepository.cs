using EcomFinale.DataAccess.Entities;

namespace EcomFinale.DataAccess.Repositories;

public interface ICartItemRepository
{
    IQueryable<CartItem> GetAll();

    Task<CartItem?> GetById(int id);

    Task<CartItem> Create(CartItem cartItem);

    Task<bool> Delete(int id);

    Task SaveChanges();
}