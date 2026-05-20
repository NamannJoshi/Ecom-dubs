using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Entities;
using EcomFinale.DataAccess.Repositories;

namespace EcomFinale.Business.Services.Implementation;

public class CartItemService : ICartItemService
{
    private readonly ICartItemRepository cartItemRepository;
    private readonly IMapper mapper;

    public CartItemService(
        ICartItemRepository cartItemRepository,
        IMapper mapper
    )
    {
        this.cartItemRepository = cartItemRepository;
        this.mapper = mapper;
    }

    public IQueryable<CartItemDto> GetAll()
    {
        return this.cartItemRepository.GetAll()
            .ProjectTo<CartItemDto>(this.mapper.ConfigurationProvider);
    }

    public async Task<CartItemDto?> GetById(int id)
    {
        var cartItem = await this.cartItemRepository.GetById(id);

        if (cartItem == null)
        {
            throw new KeyNotFoundException(
                "Cart item with matching identifier is not found"
            );
        }

        return this.mapper.Map<CartItemDto>(cartItem);
    }

    public async Task<CartItemDto> Create(CreateCartItemDto cartItemDto)
    {
        var entity = this.mapper.Map<CartItem>(cartItemDto);

        var created = await this.cartItemRepository.Create(entity);

        return this.mapper.Map<CartItemDto>(created);
    }

    public async Task<CartItemDto> Update(
        CreateCartItemDto cartItemDto,
        int id
    )
    {
        var cartItem = await this.cartItemRepository.GetById(id);

        if (cartItem == null)
        {
            throw new KeyNotFoundException(
                "Cart item with matching identifier is not found"
            );
        }

        this.mapper.Map(cartItemDto, cartItem);

        await this.cartItemRepository.SaveChanges();

        return this.mapper.Map<CartItemDto>(cartItem);
    }

    public async Task<bool> Delete(int id)
    {
        return await this.cartItemRepository.Delete(id);
    }
}