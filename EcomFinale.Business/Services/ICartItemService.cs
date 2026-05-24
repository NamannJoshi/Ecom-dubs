using EcomFinale.DataAccess.Dtos;

namespace EcomFinale.Business.Services;

public interface ICartItemService
{
    Task<CartItemDto> Create(CreateCartItemDto cartItemDto);
    Task<UpdateCartItemDto> UpdateItemQuantity(UpdateCartItemDto updateCartItemDto, int id);
    Task<bool> Delete(int id);
}