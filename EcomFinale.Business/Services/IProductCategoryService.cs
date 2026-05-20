using EcomFinale.DataAccess.Dtos;

namespace EcomFinale.Business.Services;

public interface IProductCategoryService
{
    IQueryable<ProductCategoryDto> GetAllCategories();

    Task<ProductCategoryDto> Create(ProductCategoryDto categoryDto);

    Task<ProductCategoryDto?> GetById(int id);

    Task<ProductCategoryDto> Update(ProductCategoryDto categoryDto, int id);

    Task Delete(int id);
}