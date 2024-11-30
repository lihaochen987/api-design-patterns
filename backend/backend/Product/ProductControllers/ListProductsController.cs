using backend.Product.Contracts;
using backend.Product.Services;
using backend.Product.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[Route("products")]
[ApiController]
public class ListProductsController(
    IProductViewRepository productViewRepository,
    GetProductExtensions extensions)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "List products", Tags = ["Products"])]
    public async Task<ActionResult<IEnumerable<ListProductsResponse>>> ListProducts(
        [FromQuery] ListProductsRequest request)
    {
        ProductListResult<ProductView> products = await productViewRepository.ListProductsAsync(
            request.PageToken,
            request.Filter,
            request.MaxPageSize);

        List<GetProductResponse> productResponses = products.Items.Select(extensions.ToGetProductResponse).ToList();

        ListProductsResponse response = new() { Results = productResponses, NextPageToken = products.NextPageToken };

        return Ok(response);
    }
}
