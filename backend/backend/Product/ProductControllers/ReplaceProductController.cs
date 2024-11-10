using backend.Product.Database;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class ReplaceProductController(
    ProductDbContext context,
    ReplaceProductExtensions extensions)
    : ControllerBase
{
    [HttpPut("{id:long}")]
    [SwaggerOperation(Summary = "Replace a product", Tags = ["Products"])]
    public async Task<ActionResult<ReplaceProductResponse>> ReplaceProduct(
        [FromRoute] long id,
        [FromBody] ReplaceProductRequest request)
    {
        var product = extensions.ToEntity(request);

        var existingProduct = await context.Products.FindAsync(id);
        if (existingProduct == null) return NotFound();
        existingProduct.Replace(product.Name, product.Category, product.Dimensions);
        await context.SaveChangesAsync();

        var response = extensions.ToReplaceProductResponse(product);
        return Ok(response);
    }
}