using backend.Database;
using Microsoft.AspNetCore.Mvc;

namespace backend.Product.Controllers;

[ApiController]
public class GetProductController(ApplicationDbContext context) : ControllerBase
{
    [Route("/product")]
    [HttpGet]
    public async Task<ActionResult<Product>> GetProduct([FromQuery] GetProductRequest request)
    {
        if (!long.TryParse(request.Id, out var id)) return BadRequest();
        var product = await context.Products.FindAsync(id);
        return Ok(product);
    }
}