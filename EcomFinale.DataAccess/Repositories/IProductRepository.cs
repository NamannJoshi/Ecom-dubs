using EcomFinale.DataAccess.Entities;

namespace EcomFinale.DataAccess.Repositories;

public interface IProductRepository
{
    IQueryable<Product> GetAllProducts();
    
    Task<Product> Create(Product product);

    Task DeductProductStock(int productId, int quantity);
    
    Task<Product?> GetById(int id);
    
    Task Delete(int id);
    
    Task SaveChanges();
}