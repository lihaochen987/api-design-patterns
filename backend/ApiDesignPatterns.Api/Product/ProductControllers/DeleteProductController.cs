using backend.Product.ApplicationLayer;
using backend.Product.ApplicationLayer.DeleteProduct;
using backend.Shared;
using backend.Shared.CommandService;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class DeleteProductController(
    ICommandService<DeleteProduct> service,
    IProductQueryApplicationService productQueryApplicationService)
    : ControllerBase
{
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Delete a product", Tags = ["Products"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteProduct(
        [FromRoute] long id,
        [FromQuery] DeleteProductRequest request)
    {
        DomainModels.Product? product = await productQueryApplicationService.GetProductAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        await service.Execute(new DeleteProduct { Product = product });
        return NoContent();
    }
}
