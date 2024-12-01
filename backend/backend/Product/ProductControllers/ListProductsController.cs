using AutoMapper;
using backend.Product.Contracts;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer;
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
    [ProducesResponseType(typeof(ListProductsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ListProductsResponse>>> ListProducts(
        [FromQuery] ListProductsRequest request)
    {
        ProductListResult<ProductView> products = await productViewRepository.ListProductsAsync(
            request.PageToken,
            request.Filter,
            request.MaxPageSize);

        IEnumerable<object> productResponses = products.Items.Select(product => product.Category switch
        {
            Category.PetFood => mapper.Map<GetPetFoodResponse>(product),
            Category.GroomingAndHygiene => mapper.Map<GetGroomingAndHygieneResponse>(product),
            _ => mapper.Map<GetProductResponse>(product)
        }).ToList();

        ListProductsResponse response = new() { Results = productResponses, NextPageToken = products.NextPageToken };

        return Ok(response);
    }
}
