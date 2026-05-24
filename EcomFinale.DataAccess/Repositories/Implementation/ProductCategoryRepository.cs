using EcomFinale.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcomFinale.DataAccess.Repositories.Implementation;

public class ProductCategoryRepository : IProductCategoryRepository
{
    private readonly AppDbContext context;

    public ProductCategoryRepository(AppDbContext context)
    {
        this.context = context;
    }

    public IQueryable<ProductCategory> GetAllCategories()
    {
        return this.context.ProductCategories;
    }

    public async Task<ProductCategory> Create(ProductCategory category)
    {
        var categoryResult = await this.context.ProductCategories.AddAsync(category);

        await this.context.SaveChangesAsync();

        return categoryResult.Entity;
    }

    public async Task<ProductCategory?> GetById(int id)
    {
        return await this.context.ProductCategories.FindAsync(id);
    }

    public async Task Delete(int id)
    {
        var category = await this.context.ProductCategories
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
        {
            throw new KeyNotFoundException("Category with matching identifier is not found");
        }

        this.context.ProductCategories.Remove(category);

        await this.context.SaveChangesAsync();
    }

    public async Task SaveChanges()
    {
        await this.context.SaveChangesAsync();
    }
}