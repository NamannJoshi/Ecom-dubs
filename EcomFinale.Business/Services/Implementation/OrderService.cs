using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Entities;
using EcomFinale.DataAccess.Repositories;

namespace EcomFinale.Business.Services.Implementation;

public class OrderService : IOrderService
{
    private readonly IOrderRepository orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper mapper;

    public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<OrderDto> CreateOrder(OrderDto orderCreateDto)
    {
        var order = mapper.Map<Order>(orderCreateDto);
        var createdOrder = await this.orderRepository.Create(order);

        return mapper.Map<OrderDto>(createdOrder);
    }

    public async Task<OrderDto> GetOrderById(int id)
    {
        var order = await this.orderRepository.GetById(id);
        if (order == null)
        {
            throw new KeyNotFoundException(
                "Order with matching identifier is not found"
            );
        }

        return mapper.Map<OrderDto>(order);
    }

    public IQueryable<OrderDto> GetAllOrders()
    {
        return this.orderRepository.GetAll()
            .ProjectTo<OrderDto>(mapper.ConfigurationProvider);
    }

    public async Task<OrderDto> UpdateOrder(int id, OrderDto orderUpdateDto)
    {
        var existingOrder = await this.orderRepository.GetById(id);
        if (existingOrder == null)
        {
            throw new KeyNotFoundException(
                "Order with matching identifier is not found"
            );
        }

        existingOrder.Status = orderUpdateDto.Status;
        await this.orderRepository.SaveChangesAsync();

        return mapper.Map<OrderDto>(existingOrder);
    }
}