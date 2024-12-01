using AutoMapper;
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
    IMapper mapper)
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

        List<GetProductResponse> productResponses = products.Items.Select(mapper.Map<GetProductResponse>).ToList();

        ListProductsResponse response = new() { Results = productResponses, NextPageToken = products.NextPageToken };

        return Ok(response);
    }
}
