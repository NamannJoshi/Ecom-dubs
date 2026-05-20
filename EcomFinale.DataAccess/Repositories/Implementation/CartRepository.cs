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

    public IQueryable<Cart> GetAllCarts()
    {
        return this.context.Carts;
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