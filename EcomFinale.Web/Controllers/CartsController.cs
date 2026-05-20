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

    [HttpPost]
    public async Task<ActionResult<CartDto>> CreateCart(CartDto cartDto)
    {
        var created = await this.cartService.Create(cartDto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            created
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CartDto>> UpdateCart(
        int id,
        CartDto cartDto
    )
    {
        var updated = await this.cartService.Update(cartDto, id);

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCart(int id)
    {
        await this.cartService.Delete(id);

        return NoContent();
    }
}