using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Entities;
using EcomFinale.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using EcomFinale.Business.Common;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Checkout;

namespace EcomFinale.Business.Services.Implementation;

public class OrderService : IOrderService
{
    private readonly IOrderRepository orderRepository;
    private readonly IProductRepository productRepository;
    private readonly ICartRepository cartRepository;
    private readonly ICurrentUserService currentUserService;
    private readonly IOrderItemRepository orderItemRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper mapper;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        ICartRepository cartRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        this.orderRepository = orderRepository;
        this.productRepository = productRepository;
        this.cartRepository = cartRepository;
        this.currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<OrderDto> CreateOrder(OrderDto orderCreateDto, Guid idempotencyId)
    {
        var currentUser = this.currentUserService.GetCurrentUserClaims() ?? 
            throw new UnauthorizedAccessException("Token Claims not found");

        var existingOrder = await this.orderRepository.GetByIdempotencyId(idempotencyId);
        if (existingOrder != null)
        {
            return this.mapper.Map<OrderDto>(existingOrder);
        }

        var order = new Order
        {
            UserId = currentUser.UserId,
            Status = OrderStatus.Pending,
            IdempotencyId = idempotencyId,
        };
        AuditHelper.ApplyAuditValues(order, currentUser.UserId, true);

        var currentCart = await this.cartRepository.GetByUserId(currentUser.UserId, CartStatus.Active);
        if (currentCart == null || currentCart.CartStatus != CartStatus.Active || currentCart.CartItems.Count == 0)
        {
            throw new InvalidOperationException("Cart is empty or inactive. Cannot create order.");
        }

        await this._unitOfWork.BeginTransactionAsync();

        try
        {
            foreach (var cartItem in currentCart.CartItems)
            {
                await this.productRepository.DeductProductStock(cartItem.ProductId, cartItem.Quantity);
            }

            order.OrderItems = currentCart.CartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                UnitPrice = ci.Product.Price,
            }).ToList();

            order.TotalAmount = currentCart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price);

            var createdOrder = await this.orderRepository.Create(order);

            await this._unitOfWork.Commit();
            return mapper.Map<OrderDto>(createdOrder);
        }
        catch (DbUpdateException ex)
        {
            await this._unitOfWork.Rollback();
            var concurrentOrder = await this.orderRepository.GetByIdempotencyId(idempotencyId);

            return mapper.Map<OrderDto>(concurrentOrder);
        }
        catch
        {
            await this._unitOfWork.Rollback();
            throw;
        }
    }

    public async Task OrderRollback()
    {
        var orderIds = this.orderRepository.GetAll()
            .Where(o => o.Status == OrderStatus.Pending && o.CreatedAt >= DateTime.UtcNow.AddMinutes(-10))
            .Select(o => o.Id)
            .ToList();
        foreach (var orderId in orderIds)
        {
            await ProductInventoryRollbackByOrder(orderId);
        }
    }

    public async Task Checkout(int orderId)
    {
        var currentUser = this.currentUserService.GetCurrentUserClaims() ??
            throw new UnauthorizedAccessException("Token claims not found");

        var cart = await this.cartRepository.GetByUserId(currentUser.UserId) ?? throw new KeyNotFoundException("Cart with matching identifier is not found");
        if (cart.CartStatus != CartStatus.Active)
        {
            throw new InvalidOperationException("Cart is not active. Cannot checkout.");
        }

        cart.CartStatus = CartStatus.Converted;
        cart.CheckedOutAtUtc = DateTime.UtcNow;

        var order = await this.orderRepository.GetById(orderId) ?? throw new KeyNotFoundException("Order with matching identifier is not found");
        order.Status = OrderStatus.Processing;

        await this.cartRepository.SaveChanges();
        await this.orderRepository.SaveChangesAsync();
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

    public async Task ProductInventoryRollbackByOrder(int orderId)
    {
        var order = await this.orderRepository.GetById(orderId) ?? throw new KeyNotFoundException("Order with matching identifier is not found");

            order.Status = OrderStatus.Abandoned;

            foreach (var item in order.OrderItems)
            {
                var product = await this.productRepository.GetById(item.ProductId) ?? throw new KeyNotFoundException("Product with matching identifier is not found");
                product.StockQuantity += item.Quantity;

                await this.productRepository.SaveChanges();
            }

            await this.orderRepository.SaveChangesAsync();
    }
}