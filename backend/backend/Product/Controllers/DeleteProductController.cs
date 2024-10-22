using backend.Database;
using Microsoft.AspNetCore.Mvc;

namespace backend.Product.Controllers;

[ApiController]
[Route("/product")]
public class DeleteProductController(ApplicationDbContext context) : ControllerBase
{
    [HttpDelete]
    public async Task<ActionResult> DeleteProduct([FromQuery] DeleteProductRequest request)
    {
        if (!long.TryParse(request.Id, out var id)) return BadRequest();
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