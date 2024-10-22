using backend.Database;
using Microsoft.AspNetCore.Mvc;

namespace backend.Product.Controllers;

[ApiController]
[Route("/product")]
public class GetProductController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetProductResponse>> GetProduct([FromQuery] GetProductRequest request)
    {
        if (!long.TryParse(request.Id, out var id)) return BadRequest();
        var product = await context.Products.FindAsync(id);
        if (product == null) return NotFound();

        var response = product.ToGetProductResponse();
        return Ok(response);
    }
}