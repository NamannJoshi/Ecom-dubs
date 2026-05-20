using EcomFinale.DataAccess.Dtos;

namespace EcomFinale.Business.Services;

public interface ICartItemService
{
    IQueryable<CartItemDto> GetAll();

    Task<CartItemDto?> GetById(int id);

    Task<CartItemDto> Create(CreateCartItemDto cartItemDto);

    Task<CartItemDto> Update(CreateCartItemDto cartItemDto, int id);

    Task<bool> Delete(int id);
}