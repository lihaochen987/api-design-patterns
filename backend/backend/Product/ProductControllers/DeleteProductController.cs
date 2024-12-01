using backend.Product.ApplicationLayer;
using backend.Product.InfrastructureLayer;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class DeleteProductController(IProductApplicationService applicationService) : ControllerBase
{
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Delete a product", Tags = ["Products"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteProduct(
        [FromRoute] long id,
        [FromQuery] DeleteProductRequest request)
    {
        GetProductResponse? product = await applicationService.GetProductAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        await applicationService.DeleteProductAsync(product);
        return NoContent();
    }
}
