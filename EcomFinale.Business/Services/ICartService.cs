using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Entities;

namespace EcomFinale.Business.Services;

public interface ICartService
{
    IQueryable<CartDto> GetAllCarts(CartStatus? status = null);

    Task<CartDto?> GetById(int id);

    Task<CartDto> Update(CartDto cartDto, int id);

    Task<CartDto> CheckoutCart(int id);

    Task Delete(int id);
}