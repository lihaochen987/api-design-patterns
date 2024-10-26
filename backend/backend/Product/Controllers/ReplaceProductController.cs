using backend.Database;
using Microsoft.AspNetCore.Mvc;

namespace backend.Product.Controllers;

[ApiController]
[Route("product")]
public class ReplaceProductController(ApplicationDbContext context) : ControllerBase
{
    [HttpPut("{id:long}")]
    public async Task<ActionResult<ReplaceProductResponse>> ReplaceProduct(
        [FromRoute] long id,
        [FromBody] ReplaceProductRequest request)
    {
        var product = request.ToEntity();

        var existingProduct = await context.Products.FindAsync(id);
        if (existingProduct == null) return NotFound();
        existingProduct.Replace(product.Name, product.Price, product.Category, product.Dimensions);
        await context.SaveChangesAsync();

        var response = product.ToReplaceProductResponse();
        return Ok(response);
    }
}