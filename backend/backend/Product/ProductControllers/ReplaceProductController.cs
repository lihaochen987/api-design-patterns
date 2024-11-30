using backend.Product.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class ReplaceProductController(
    IProductRepository productRepository,
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

        var existingProduct = await productRepository.GetProductAsync(id);
        if (existingProduct == null) return NotFound();

        await productRepository.ReplaceProductAsync(existingProduct);

        var response = extensions.ToReplaceProductResponse(product);
        return Ok(response);
    }
}