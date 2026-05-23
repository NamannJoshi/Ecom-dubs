using AutoMapper;
using Microsoft.AspNetCore.Http;
using AutoMapper.QueryableExtensions;
using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Entities;
using EcomFinale.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using EcomFinale.Business.Common;

namespace EcomFinale.Business.Services.Implementation;

public class OrderService : IOrderService
{
    private readonly IOrderRepository orderRepository;
    private readonly IProductRepository productRepository;
    private readonly ICartRepository cartRepository;
    private readonly IOrderItemRepository orderItemRepository;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper mapper;

    public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, ICartRepository cartRepository, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.orderRepository = orderRepository;
        this.productRepository = productRepository;
        this.cartRepository = cartRepository;
        this.httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<OrderDto> CreateOrder(OrderDto orderCreateDto, Guid idempotencyId)
    {
        var existingOrder = await this.orderRepository.GetByIdempotencyId(idempotencyId);
        if (existingOrder != null)
        {
            // return this.mapper.Map<OrderDto>(existingOrder);
        }

        var order = new Order
        {
            UserId = 1,
            Status = OrderStatus.Pending,
            IdempotencyId = idempotencyId,
        };
        AuditHelper.ApplyAuditValues(order, true);

       try
        {
            var currentCart = await this.cartRepository.GetByUserId(1);
            if (currentCart == null || currentCart.CartStatus != CartStatus.Active || currentCart.CartItems.Count == 0)
            {
                throw new InvalidOperationException("Cart is empty or inactive. Cannot create order.");
            }
            order.OrderItems = currentCart.CartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                UnitPrice = ci.Product.Price,
            }).ToList();
           
            order.TotalAmount = currentCart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price);

            var createdOrder = await this.orderRepository.Create(order);
            return mapper.Map<OrderDto>(createdOrder);
        }
        catch (DbUpdateException ex) 
        {
            var concurrentOrder = await this.orderRepository.GetByIdempotencyId(idempotencyId);
            return mapper.Map<OrderDto>(concurrentOrder);
        }
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