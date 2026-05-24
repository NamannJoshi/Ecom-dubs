using EcomFinale.DataAccess.Dtos;

namespace EcomFinale.Business.Services;

public interface IOrderService
{
    Task<OrderDto> CreateOrder(OrderDto orderCreateDto, Guid idempotencyId);
    Task<OrderDto> GetOrderById(int id);
    IQueryable<OrderDto> GetAllOrders();
    Task<OrderDto> UpdateOrder(int id, OrderDto orderUpdateDto);
    Task OrderRollback();
    Task Checkout(int orderId);
}