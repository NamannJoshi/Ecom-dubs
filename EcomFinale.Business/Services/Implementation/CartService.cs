using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcomFinale.Business.Common;
using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Entities;
using EcomFinale.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

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

    public async Task<CartDto> Create(CartDto cartDto)
    {
        var activeCartExists = await this.cartRepository.GetAllCarts()
            .AnyAsync(cart => cart.CartStatus == CartStatus.Active && cart.UserId == cartDto.UserId);
        if (activeCartExists)
        {
            throw new InvalidOperationException(
                "An active cart already exists for the user. Please checkout or delete the existing cart before creating a new one."
            );
        }

        var entity = this.mapper.Map<Cart>(cartDto);

        // AuditHelper.ApplyAuditValues(entity, true);
        var created = await this.cartRepository.Create(entity);

        return this.mapper.Map<CartDto>(created);
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

        this.mapper.Map(cartDto, cart);

        await this.cartRepository.SaveChanges();

        return this.mapper.Map<CartDto>(cart);
    }

    public async Task Delete(int id)
    {
        await this.cartRepository.Delete(id);
    }
}