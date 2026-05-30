using EcomFinale.Business.Services;
using EcomFinale.DataAccess.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace EcomFinale.Web.Controllers;

// [Authorize]
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await this.orderService.GetOrderById(id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet]
    public IActionResult GetAllOrders()
    {
        var orders = this.orderService.GetAllOrders();
        return Ok(orders);
    }

    [AllowAnonymous]
    [HttpPost("success")]
    public async Task PaymentSuccess()
    {
        Console.WriteLine("Payment was successful!");
    }

    [AllowAnonymous]
    [HttpPost("failed")]
    public async Task PaymentFailed()
    {
        Console.WriteLine("Payment failed!");
    }
}