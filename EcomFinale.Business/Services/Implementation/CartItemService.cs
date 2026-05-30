using AutoMapper;
using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Entities;
using EcomFinale.DataAccess.Repositories;

namespace EcomFinale.Business.Services.Implementation;

public class CartItemService : ICartItemService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ICartItemRepository cartItemRepository;
    private readonly ICartRepository cartRepository;
    private readonly IProductRepository productRepository;
    private readonly IMapper mapper;

    public CartItemService(
        IUnitOfWork unitOfWork,
        ICartItemRepository cartItemRepository,
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IMapper mapper
    )
    {
        this.unitOfWork = unitOfWork;
        this.cartItemRepository = cartItemRepository;
        this.cartRepository = cartRepository;
        this.productRepository = productRepository;
        this.mapper = mapper;
    }
  
    public async Task<CartItemDto> Create(CreateCartItemDto cartItemDto)
    {
        var currentUserId = 9;
        var cart = await this.cartRepository.GetByUserId(currentUserId);

        var product = await this.productRepository.GetById(cartItemDto.ProductId) ??
                throw new KeyNotFoundException("Product with matching identifier is not found");
            
        if (product.StockQuantity < cartItemDto.Quantity)
        {
            throw new InvalidOperationException("Requested quantity is not available in stock");
        }

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