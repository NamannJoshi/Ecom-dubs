using EcomFinale.DataAccess.Entities;

namespace EcomFinale.DataAccess.Repositories;

public interface IProductCategoryRepository
{
    IQueryable<ProductCategory> GetAllCategories();

    Task<ProductCategory> Create(ProductCategory category);

    Task<ProductCategory?> GetById(int id);

    Task Delete(int id);

    Task SaveChanges();
}