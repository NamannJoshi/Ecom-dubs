using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Entities;
using EcomFinale.DataAccess.Repositories;

namespace EcomFinale.Business.Services.Implementation;

public class CartService : ICartService
{
    private readonly ICartRepository cartRepository;
    private readonly IMapper mapper;

    public CartService(
        ICartRepository cartRepository,
        IMapper mapper
    )
    {
        this.cartRepository = cartRepository;
        this.mapper = mapper;
    }

    public IQueryable<CartDto> GetAllCarts(CartStatus? status = null)
    {
        var query = this.cartRepository.GetAllCarts();

        if (status.HasValue)
        {
            query = query.Where(cart => cart.CartStatus == status.Value);
        }

        return query
            .ProjectTo<CartDto>(this.mapper.ConfigurationProvider);
    }

    public async Task<CartDto?> GetById(int id)
    {
        var cart = await this.cartRepository.GetById(id);

        if (cart == null)
        {
            throw new KeyNotFoundException(
                "Cart with matching identifier is not found"
            );
        }

        return this.mapper.Map<CartDto>(cart);
    }

    public async Task<CartDto> Update(CartDto cartDto, int id)
    {
        var cart = await this.cartRepository.GetById(id);

        if (cart == null)
        {
            throw new KeyNotFoundException(
                "Cart with matching identifier is not found"
            );
        }

        cart.CartStatus = cartDto.CartStatus;
        await this.cartRepository.SaveChanges();

        return this.mapper.Map<CartDto>(cart);
    }

    public async Task<CartDto> CheckoutCart(int id)
    {
        var cartDto = new CartDto
        {
            CartStatus = CartStatus.Converted
        };
        return await this.Update(cartDto, id);
    }
  
    public async Task Delete(int id)
    {
        await this.cartRepository.Delete(id);
    }
}