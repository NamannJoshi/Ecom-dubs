using AutoMapper;
using EcomFinale.Business.Common;
using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Entities;
using EcomFinale.DataAccess.Repositories;

namespace EcomFinale.Business.Services.Implementation;

public class ProductCategoryService : IProductCategoryService
{
    private readonly IProductCategoryRepository productCategoryRepository;
    private readonly IMapper mapper;

    public ProductCategoryService(
        IProductCategoryRepository productCategoryRepository,
        IMapper mapper
    )
    {
        this.productCategoryRepository = productCategoryRepository;
        this.mapper = mapper;
    }

    public IQueryable<ProductCategoryDto> GetAllCategories()
    {
        return this.productCategoryRepository.GetAllCategories()
            .Select(category => new ProductCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            });
    }

    public async Task<ProductCategoryDto> Create(ProductCategoryDto categoryDto)
    {
        var entity = this.mapper.Map<ProductCategory>(categoryDto);
        AuditHelper.ApplyAuditValues(entity, true);
        var utc = DateTime.UtcNow;
        entity.CreatedBy = 1;
        entity.ModifiedBy = 1;

        var created = await this.productCategoryRepository.Create(entity);

        return this.mapper.Map<ProductCategoryDto>(created);
    }

    public async Task<ProductCategoryDto?> GetById(int id)
    {
        var category = await this.productCategoryRepository.GetById(id);

        if (category == null)
        {
            throw new KeyNotFoundException(
                "Category with matching identifier is not found"
            );
        }

        return this.mapper.Map<ProductCategoryDto>(category);
    }

    public async Task<ProductCategoryDto> Update(
        ProductCategoryDto categoryDto,
        int id
    )
    {
        var category = await this.productCategoryRepository.GetById(id);

        if (category == null)
        {
            throw new KeyNotFoundException(
                "Category with matching identifier is not found"
            );
        }

        this.mapper.Map(categoryDto, category);
        AuditHelper.ApplyAuditValues(category, false);

        await this.productCategoryRepository.SaveChanges();

        return this.mapper.Map<ProductCategoryDto>(category);
    }

    public async Task Delete(int id)
    {
        await this.productCategoryRepository.Delete(id);
    }
}