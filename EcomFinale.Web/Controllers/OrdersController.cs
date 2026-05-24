using EcomFinale.Business.Services;
using EcomFinale.DataAccess.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EcomFinale.Web.Controllers;

[ApiController]
[Route("api/Orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService orderService;

    public OrdersController(IOrderService orderService)
    {
        this.orderService = orderService;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, OrderDto orderUpdateDto)
    {
        var updatedOrder = await this.orderService.UpdateOrder(id, orderUpdateDto);
        return Ok(updatedOrder);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderCreateDto, [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey)
    {
        var idempotencyId = idempotencyKey ?? throw new ArgumentException("Idempotency-Key header is required");

        if (!Guid.TryParse(idempotencyId, out var parsedIdempotencyId))
        {
            throw new ArgumentException("Idempotency-Key header must be a valid GUID");
        }
        var createdOrder = await this.orderService.CreateOrder(orderCreateDto, parsedIdempotencyId);
        return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await this.orderService.GetOrderById(id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpGet]
    public IActionResult GetAllOrders()
    {
        var orders = this.orderService.GetAllOrders();
        return Ok(orders);
    }
}