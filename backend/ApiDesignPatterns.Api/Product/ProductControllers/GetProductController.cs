using AutoMapper;
using backend.Product.ApplicationLayer;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.Views;
using backend.Product.Services;
using backend.Product.Services.ProductServices;
using backend.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class GetProductController(
    IProductViewApplicationService productViewApplicationService,
    ProductFieldPaths fieldPaths,
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
        ProductView? productView = await productViewApplicationService.GetProductView(id);
        if (productView == null)
        {
            return NotFound();
        }

        GetProductResponse response = productView.Category switch
        {
            Category.PetFood => mapper.Map<GetPetFoodResponse>(productView),
            Category.GroomingAndHygiene => mapper.Map<GetGroomingAndHygieneResponse>(productView),
            _ => mapper.Map<GetProductResponse>(productView)
        };
        JsonSerializerSettings settings = new()
        {
            Converters = new List<JsonConverter>
            {
                new FieldMaskConverter(request.FieldMask, fieldPaths.ValidFields)
            }
        };
        string json = JsonConvert.SerializeObject(response, settings);

        return new OkObjectResult(json) { StatusCode = 200 };
    }
}
