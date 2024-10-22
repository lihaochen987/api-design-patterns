using backend.Database;
using Microsoft.AspNetCore.Mvc;

namespace backend.Product.Controllers;

[ApiController]
public class ReplaceProductController(ApplicationDbContext context) : ControllerBase
{
    [Route("/product")]
    [HttpPut]
    public async Task<ActionResult<Product>> ReplaceProduct([FromQuery] long id,
        [FromBody] ReplaceProductRequest request)
    {
        var existingProduct = await context.Products.FindAsync(id);
        if (existingProduct == null) return NotFound();

        existingProduct.UpdateProductDetails(request, out var errorMessages);

        if (errorMessages.Any())
        {
            return BadRequest(new { Errors = errorMessages });
        }

        await context.SaveChangesAsync();

        return Ok(existingProduct);
    }
}