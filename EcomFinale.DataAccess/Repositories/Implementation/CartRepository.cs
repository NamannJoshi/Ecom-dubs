using EcomFinale.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcomFinale.DataAccess.Repositories.Implementation;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext context;

    public CartRepository(AppDbContext context)
    {
        this.context = context;
    }

    public IQueryable<Cart> GetAllCarts(CartStatus? status = null)
    {
        var query = this.context.Carts.AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(c => c.CartStatus == status.Value);
        }

        return query;
    }

    public async Task<Cart> Create(Cart cart)
    {
        var cartResult = await this.context.Carts.AddAsync(cart);

        await this.context.SaveChangesAsync();

        return cartResult.Entity;
    }

    public async Task<Cart?> GetById(int id)
    {
        return await this.context.Carts.FindAsync(id);
    }

    public async Task<Cart?> GetByUserId(int userId, CartStatus? status = null)
    {
        var query = this.GetAllCarts(status)
                        .Include(c => c.CartItems)
                            .ThenInclude(ci => ci.Product)
                        .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(o => o.CartStatus == status);
        }
          
        return await query.FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task Delete(int id)
    {
        var cart = await this.context.Carts
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cart == null)
        {
            throw new KeyNotFoundException(
                "Cart with matching identifier is not found"
            );
        }

        this.context.Carts.Remove(cart);

        await this.context.SaveChangesAsync();
    }

    public async Task SaveChanges()
    {
        await this.context.SaveChangesAsync();
    }
}