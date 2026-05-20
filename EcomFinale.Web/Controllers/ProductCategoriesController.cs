using EcomFinale.Business.Services;
using EcomFinale.DataAccess.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EcomFinale.Web.Controllers;

[ApiController]
[Route("api/ProductCategories")]
public class ProductCategoriesController : ControllerBase
{
    private readonly IProductCategoryService productCategoryService;

    public ProductCategoriesController(
        IProductCategoryService productCategoryService
    )
    {
        this.productCategoryService = productCategoryService;
    }

    [HttpGet]
    public ActionResult<IQueryable<ProductCategoryDto>> GetAllCategories()
    {
        var categories = this.productCategoryService.GetAllCategories();

        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductCategoryDto>> GetById(int id)
    {
        var category = await this.productCategoryService.GetById(id);

        if (category == null)
        {
            return NotFound();
        }

        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<ProductCategoryDto>> CreateCategory(
        ProductCategoryDto categoryDto
    )
    {
        var created = await this.productCategoryService.Create(categoryDto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            created
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductCategoryDto>> UpdateCategory(
        int id,
        ProductCategoryDto categoryDto
    )
    {
        var updated = await this.productCategoryService.Update(
            categoryDto,
            id
        );

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        await this.productCategoryService.Delete(id);

        return NoContent();
    }
}