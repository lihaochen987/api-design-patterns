using AutoMapper;
using backend.Product.ApplicationLayer;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.Views;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[Route("products")]
[ApiController]
public class ListProductsController(
    IProductViewQueryApplicationService queryApplicationService,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "List products", Tags = ["Products"])]
    [ProducesResponseType(typeof(ListProductsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ListProductsResponse>>> ListProducts(
        [FromQuery] ListProductsRequest request)
    {
        (List<ProductView> products, string? nextPageToken) = await queryApplicationService.ListProductsAsync(request);

        IEnumerable<GetProductResponse> productResponses = products.Select(product => product.Category switch
        {
            Category.PetFood => mapper.Map<GetPetFoodResponse>(product),
            Category.GroomingAndHygiene => mapper.Map<GetGroomingAndHygieneResponse>(product),
            _ => mapper.Map<GetProductResponse>(product)
        }).ToList();

        ListProductsResponse response = new() { Results = productResponses, NextPageToken = nextPageToken };

        return Ok(response);
    }
}
