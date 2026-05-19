using EcomFinale.DataAccess.Dtos;

namespace EcomFinale.Business.Services;

public interface IProductService
{
    IQueryable<ProductDto> GetAllProducts();

    Task<ProductDto> Create(ProductDto productDto);

    Task<ProductDto?> GetById(int id);

    Task<ProductDto> Update(ProductDto productDto, int id);

    Task Delete(int id);
}