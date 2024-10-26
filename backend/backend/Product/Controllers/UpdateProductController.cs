using backend.Database;
using Microsoft.AspNetCore.Mvc;

namespace backend.Product.Controllers;

[ApiController]
[Route("product")]
public class UpdateProductController(ApplicationDbContext context) : ControllerBase
{
    // Todo: Actually implement partial Updates because it's hard
    [HttpPatch("{id:long}")]
    public async Task<ActionResult<UpdateProductResponse>> UpdateProduct(
        [FromRoute] long id,
        [FromBody] UpdateProductRequest request)
    {
        var product = await context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        await context.SaveChangesAsync();
        return Ok(product);
    }
}