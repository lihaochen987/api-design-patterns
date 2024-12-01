using AutoMapper;
using backend.Product.ApplicationLayer;
using backend.Product.DomainModels.Enums;
using backend.Product.Services;
using backend.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class GetProductController(
    IProductViewApplicationService productViewApplicationService,
    ProductFieldMaskConfiguration configuration,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Get a product", Tags = ["Products"])]
    [ProducesResponseType(typeof(GetProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetProductResponse>> GetProduct(
        [FromRoute] long id,
        [FromQuery] GetProductRequest request)
    {
        string? json = await productViewApplicationService.GetProductView(id, request);
        if (json == null)
        {
            return NotFound();
        }

        return new OkObjectResult(json) { StatusCode = 200 };
    }
}
