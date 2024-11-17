using backend.Product.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[Route("products")]
[ApiController]
public class ListProductsController(
    IProductRepository productRepository,
    GetProductExtensions extensions)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "List products", Tags = ["Products"])]
    public async Task<ActionResult<IEnumerable<ListProductsResponse>>> ListProducts(
        [FromQuery] ListProductsRequest request)
    {
        var products = await productRepository.ListProductsAsync(
            request.PageToken,
            request.Filter,
            request.MaxPageSize);

        var productResponses = products.Items.Select(extensions.ToGetProductResponse).ToList();

        var response = new ListProductsResponse
        {
            Results = productResponses,
            NextPageToken = products.NextPageToken
        };

        return Ok(response);
    }
}