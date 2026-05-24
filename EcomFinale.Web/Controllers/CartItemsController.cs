using EcomFinale.Business.Services;
using EcomFinale.DataAccess.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EcomFinale.Web.Controllers;

[ApiController]
[Route("api/CartItems")]
public class CartItemsController : ControllerBase
{
    private readonly ICartItemService cartItemService;

    public CartItemsController(ICartItemService cartItemService)
    {
        this.cartItemService = cartItemService;
    }

    [HttpPost]
    public async Task<ActionResult<CartItemDto>> Create(
        [FromBody] CreateCartItemDto cartItemDto
    )
    {
        var created = await this.cartItemService.Create(cartItemDto);
        return Created($"/api/cartitems/{created.Id}", created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UpdateCartItemDto>> UpdateItemQuantity(
        int id,
        [FromBody] UpdateCartItemDto updateCartItemDto
    )
    {
        var updated = await this.cartItemService.UpdateItemQuantity(updateCartItemDto, id);

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await this.cartItemService.Delete(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}