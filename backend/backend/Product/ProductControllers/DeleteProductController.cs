using backend.Product.Database;
using backend.Product.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class DeleteProductController(IProductRepository productRepository) : ControllerBase
{
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Delete a product", Tags = ["Products"])]
    public async Task<ActionResult> DeleteProduct(
        [FromRoute] long id,
        [FromQuery] DeleteProductRequest request)
    {
        var product = await productRepository.GetProductAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        await productRepository.DeleteProductAsync(product);
        return NoContent();
    }
}