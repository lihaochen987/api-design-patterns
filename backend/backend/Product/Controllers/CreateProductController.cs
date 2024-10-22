using backend.Database;
using Microsoft.AspNetCore.Mvc;

namespace backend.Product.Controllers;

[ApiController]
public class CreateProductController(ApplicationDbContext context) : ControllerBase
{
    [Route("/product")]
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductRequest request)
    {
        if (!Product.TryParse(request, out var product, out var errorMessages) || product == null)
            return BadRequest(errorMessages);
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return CreatedAtAction("GetProduct", new { id = product.Id }, product);
    }
}