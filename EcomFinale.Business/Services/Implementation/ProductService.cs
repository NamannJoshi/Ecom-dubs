using AutoMapper;
using EcomFinale.DataAccess.Repositories;
using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Entities;
using EcomFinale.Business.Common;

namespace EcomFinale.Business.Services.Implementation;

public class ProductService : IProductService
{
    private readonly IProductRepository productRepository;
    private readonly ICurrentUserService currentUserService;
    private readonly IMapper mapper;

    public ProductService(IProductRepository productRepository, ICurrentUserService currentUserService, IMapper mapper)
    {
        this.productRepository = productRepository;
        this.currentUserService = currentUserService;
        this.mapper = mapper;
    }

    public IQueryable<ProductDto> GetAllProducts()
    {
        return this.productRepository.GetAllProducts()
            .Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity
            });
    }

    public async Task<ProductDto> Create(ProductDto productDto)
    {
        var currentUser = this.currentUserService.GetCurrentUserClaims();
        var entity = this.mapper.Map<Product>(productDto);

        AuditHelper.ApplyAuditValues(entity, currentUser.UserId, true);
        var created = await this.productRepository.Create(entity);

        return this.mapper.Map<ProductDto>(created);
    }

    public async Task<ProductDto?> GetById(int id)
    {
        var product = await this.productRepository.GetById(id);

        if (product == null)
        {
            throw new KeyNotFoundException("Product with matching identifier is not found");
        }

        return this.mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> Update(ProductDto productDto, int id)
    {
        var currentUser = this.currentUserService.GetCurrentUserClaims();
        var product = await this.productRepository.GetById(id);

        if (product == null)
        {
            throw new KeyNotFoundException("Product with matching identifier is not found");
        }

        this.mapper.Map(productDto, product);
        AuditHelper.ApplyAuditValues(product, currentUser.UserId, false);

        await this.productRepository.SaveChanges();

        return this.mapper.Map<ProductDto>(product);
    }

    public async Task Delete(int id)
    {
        await this.productRepository.Delete(id);
    }
}