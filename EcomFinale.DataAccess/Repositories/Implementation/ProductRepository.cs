using EcomFinale.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcomFinale.DataAccess.Repositories.Implementation;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext context;

    public ProductRepository(AppDbContext context)
    {
        this.context = context;
    }

    public IQueryable<Product> GetAllProducts()
    {
        return this.context.Products;
    }

    public async Task<Product> Create(Product product)
    {
        var productResult = await this.context.Products.AddAsync(product);
        await this.context.SaveChangesAsync();
        return productResult.Entity;
    }

    public async Task<Product?> GetById(int id)
    {
        return await this.context.Products.FindAsync(id);
    }

    public async Task Delete(int id)
    {
        var product = await this.context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            throw new KeyNotFoundException("Product with matching identifier is not found");
        }
        this.context.Products.Remove(product);
        await this.context.SaveChangesAsync();
    }

    public async Task SaveChanges()
    {
        await this.context.SaveChangesAsync();
    }
}