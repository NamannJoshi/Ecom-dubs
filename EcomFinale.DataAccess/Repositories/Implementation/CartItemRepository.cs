using EcomFinale.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcomFinale.DataAccess.Repositories.Implementation;

public class CartItemRepository : ICartItemRepository
{
    private readonly AppDbContext context;

    public CartItemRepository(AppDbContext context)
    {
        this.context = context;
    }

    public IQueryable<CartItem> GetAll()
    {
        return this.context.CartItems;
    }

    public async Task<CartItem?> GetById(int id)
    {
        return await this.context.CartItems
            .FirstOrDefaultAsync(cartItem => cartItem.Id == id);
    }

    public async Task<CartItem> Create(CartItem cartItem)
    {
        await this.context.CartItems.AddAsync(cartItem);

        await this.context.SaveChangesAsync();

        return cartItem;
    }

    public async Task<bool> Delete(int id)
    {
        var cartItem = await this.context.CartItems
            .FirstOrDefaultAsync(cartItem => cartItem.Id == id);

        if (cartItem == null)
        {
            return false;
        }

        this.context.CartItems.Remove(cartItem);

        await this.context.SaveChangesAsync();

        return true;
    }

    public async Task SaveChanges()
    {
        await this.context.SaveChangesAsync();
    }
}