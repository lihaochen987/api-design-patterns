using backend.Product.ApplicationLayer;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[Route("products")]
[ApiController]
public class ListProductsController(
    IProductViewApplicationService applicationService)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "List products", Tags = ["Products"])]
    [ProducesResponseType(typeof(ListProductsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ListProductsResponse>>> ListProducts(
        [FromQuery] ListProductsRequest request)
    {
        var products = await applicationService.ListProductsAsync(request);
        return Ok(products);
    }
}
