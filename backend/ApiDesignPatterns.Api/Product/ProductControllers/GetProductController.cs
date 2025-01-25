using AutoMapper;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.Views;
using backend.Product.Queries.GetProductView;
using backend.Shared.FieldMask;
using backend.Shared.FieldPath;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class GetProductController(
    IQueryHandler<GetProductViewQuery, ProductView> getProductView,
    IFieldPathAdapter fieldPathAdapter,
    IFieldMaskConverterFactory fieldMaskConverterFactory,
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
        ProductView? productView = await getProductView.Handle(new GetProductViewQuery { Id = id });
        if (productView == null)
        {
            return NotFound();
        }

        GetProductResponse response = Enum.Parse<Category>(productView.Category) switch
        {
            Category.PetFood => mapper.Map<GetPetFoodResponse>(productView),
            Category.GroomingAndHygiene => mapper.Map<GetGroomingAndHygieneResponse>(productView),
            _ => mapper.Map<GetProductResponse>(productView)
        };


        HashSet<string> validPaths = fieldPathAdapter.GetFieldPaths("Product");
        var converter = fieldMaskConverterFactory.Create(request.FieldMask, validPaths);
        JsonSerializerSettings settings = new() { Converters = new List<JsonConverter> { converter } };
        string json = JsonConvert.SerializeObject(response, settings);

        return new OkObjectResult(json) { StatusCode = 200 };
    }
}
