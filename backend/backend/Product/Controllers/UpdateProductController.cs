using backend.Database;
using Microsoft.AspNetCore.Mvc;

namespace backend.Product.Controllers;

[ApiController]
public class UpdateProductController(ApplicationDbContext context) : ControllerBase
{
    // Todo: Actually implement partial Updates because it's hard
    [Route("/product")]
    [HttpPatch]
    public async Task<ActionResult<Product>> UpdateProduct([FromQuery] long id, [FromBody] UpdateProductRequest request)
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