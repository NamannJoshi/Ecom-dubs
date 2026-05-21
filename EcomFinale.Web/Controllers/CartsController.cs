using EcomFinale.Business.Services;
using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcomFinale.Web.Controllers;

[ApiController]
[Route("api/Carts")]
public class CartsController : ControllerBase
{
    private readonly ICartService cartService;

    public CartsController(ICartService cartService)
    {
        this.cartService = cartService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CartDto>>> GetAllCarts([FromQuery] CartStatus? status = null)
    {
        var carts = await this.cartService
            .GetAllCarts(status)
            .ToListAsync();

        return Ok(carts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CartDto>> GetById(int id)
    {
        var cart = await this.cartService.GetById(id);
        if (cart == null)
        {
            return NotFound();
        }
        return Ok(cart);
    }

    [HttpPut("{id}/checkout")]
    public async Task<ActionResult<CartDto>> CheckoutCart(
        int id
    )
    {
        var updated = await this.cartService.CheckoutCart(id);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCart(int id)
    {
        await this.cartService.Delete(id);
        return NoContent();
    }
}