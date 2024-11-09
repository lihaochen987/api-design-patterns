using backend.Database;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class DeleteProductController(ApplicationDbContext context) : ControllerBase
{
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Delete a product", Tags = ["Products"])]
    public async Task<ActionResult> DeleteProduct(
        [FromRoute] long id,
        [FromQuery] DeleteProductRequest request)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return NoContent();
    }
}