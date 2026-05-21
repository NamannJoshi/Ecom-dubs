using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Entities;
using EcomFinale.DataAccess.Enums;
using EcomFinale.DataAccess.Repositories;

namespace EcomFinale.Business.Services.Implementation;

public class CartItemService : ICartItemService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ICartItemRepository cartItemRepository;
    private readonly ICartRepository cartRepository;
    private readonly IMapper mapper;

    public CartItemService(
        IUnitOfWork unitOfWork,
        ICartItemRepository cartItemRepository,
        ICartRepository cartRepository,
        IMapper mapper
    )
    {
        this.unitOfWork = unitOfWork;
        this.cartItemRepository = cartItemRepository;
        this.cartRepository = cartRepository;
        this.mapper = mapper;
    }
  
    public async Task<CartItemDto> Create(CreateCartItemDto cartItemDto)
    {
        var currentUserId = 1;
        var cart = await this.cartRepository.GetByUserId(currentUserId);

        await this.unitOfWork.BeginTransactionAsync();

        try
        {
            if (cart == null || cart.CartStatus != CartStatus.Converted)
            {
                var utc = DateTime.UtcNow;

                if (cart != null && cart.CartStatus == CartStatus.Abandoned)
                {
                    cart.CartStatus = CartStatus.Active;
                    cart.ModifiedAt = utc;
                }
                else {
                    cart = await this.cartRepository.Create(new Cart
                    {
                        UserId = currentUserId,
                        CreatedAt = utc,
                        ModifiedAt = utc,
                    });
                }
            }

            var entity = this.mapper.Map<CartItem>(cartItemDto);
            entity.CartId = cart.Id;

            var created = await this.cartItemRepository.Create(entity);

            await this.unitOfWork.Commit();
            return this.mapper.Map<CartItemDto>(created);
        }
        catch (Exception)
        {
            await this.unitOfWork.Rollback();
            throw;
        }
    }

    public async Task<UpdateCartItemDto> UpdateItemQuantity(UpdateCartItemDto updateCartItemDto, int id)
    {
        var cartItem = await this.cartItemRepository.GetById(id) ??
                throw new KeyNotFoundException("Cart item with matching identifier is not found");

        if (updateCartItemDto.Quantity > 0)
        {
            if (cartItem.Quantity + updateCartItemDto.Quantity > cartItem.Product.StockQuantity)
            {
                throw new InvalidOperationException(
                    "Cannot increment quantity beyond available stock"
                );
            }
            cartItem.Quantity += updateCartItemDto.Quantity;
        }
        else
        {
            if (cartItem.Quantity + updateCartItemDto.Quantity < 0)
            {
                await this.cartItemRepository.Delete(cartItem.Id);
                return this.mapper.Map<UpdateCartItemDto>(cartItem);
            }
            cartItem.Quantity += updateCartItemDto.Quantity;
        }

        await this.cartItemRepository.SaveChanges();
        return this.mapper.Map<UpdateCartItemDto>(cartItem);
    }

    public async Task<bool> Delete(int id)
    {
        return await this.cartItemRepository.Delete(id);
    }
}