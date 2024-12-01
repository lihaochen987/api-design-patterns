using AutoMapper;
using backend.Product.DomainModels.Enums;
using backend.Product.FieldMasks;
using backend.Product.Services;
using backend.Product.ViewModels;
using backend.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class GetProductController(
    IProductViewRepository productViewRepository,
    ProductFieldMaskConfiguration configuration,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Get a product", Tags = ["Products"])]
    public async Task<ActionResult<GetProductResponse>> GetProduct(
        [FromRoute] long id,
        [FromQuery] GetProductRequest request)
    {
        ProductView? product = await productViewRepository.GetProductView(id);
        if (product == null)
        {
            return NotFound();
        }

        object response = product.Category switch
        {
            Category.PetFood => mapper.Map<GetPetFoodResponse>(product),
            Category.GroomingAndHygiene => mapper.Map<GetGroomingAndHygieneResponse>(product),
            _ => mapper.Map<GetProductResponse>(product)
        };

        JsonSerializerSettings settings = new()
        {
            Converters = new List<JsonConverter>
            {
                new FieldMaskConverter(request.FieldMask, configuration.ProductFieldPaths)
            }
        };

        string json = JsonConvert.SerializeObject(response, settings);

        return new OkObjectResult(json) { StatusCode = 200 };
    }
}
