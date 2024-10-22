using backend.Database;
using Microsoft.AspNetCore.Mvc;

namespace backend.Product.Controllers;

[ApiController]
public class ReplaceProductController(ApplicationDbContext context) : ControllerBase
{
    [Route("/product")]
    [HttpPut]
    public async Task<ActionResult<ReplaceProductResponse>> ReplaceProduct(
        [FromQuery] long id,
        [FromBody] ReplaceProductRequest request)
    {
        var product = ProductMapper.MapToDomain(request);

        var existingProduct = await context.Products.FindAsync(id);
        if (existingProduct == null) return NotFound();
        existingProduct.Replace(product.Name, product.Price, product.Category);
        await context.SaveChangesAsync();

        var response = ProductMapper.MapToReplaceProductResponse(existingProduct);
        return Ok(response);
    }
}