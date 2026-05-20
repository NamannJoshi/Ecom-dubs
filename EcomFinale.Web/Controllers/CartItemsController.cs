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

    [HttpGet]
    public ActionResult<IQueryable<CartItemDto>> GetAll()
    {
        return Ok(this.cartItemService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CartItemDto>> GetById(int id)
    {
        var cartItem = await this.cartItemService.GetById(id);

        return Ok(cartItem);
    }

    [HttpPost]
    public async Task<ActionResult<CartItemDto>> Create(
        [FromBody] CreateCartItemDto cartItemDto
    )
    {
        var created = await this.cartItemService.Create(cartItemDto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            created
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CartItemDto>> Update(
        int id,
        [FromBody] CreateCartItemDto cartItemDto
    )
    {
        var updated = await this.cartItemService.Update(cartItemDto, id);

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