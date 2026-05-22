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
    public async Task<IActionResult> CreateOrder(OrderDto orderCreateDto)
    {
        var createdOrder = await this.orderService.CreateOrder(orderCreateDto);
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