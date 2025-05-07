using backend.Product.ApplicationLayer.Queries.GetProductPricing;
using backend.Product.DomainModels.Views;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.Controllers.ProductPricing;

[ApiController]
[Route("product")]
public class GetProductPricingController(
    IAsyncQueryHandler<GetProductPricingQuery, ProductPricingView?> getProductPricing,
    IMapper mapper,
    IFieldMaskConverterFactory fieldMaskConverterFactory)
    : ControllerBase
{
    [HttpGet("{id:long}/pricing")]
    [SwaggerOperation(Summary = "Get a product pricing", Tags = ["Products, ProductPricing"])]
    public async Task<ActionResult<GetProductPricingResponse>> GetProductPricing(
        [FromRoute] long id,
        [FromQuery] GetProductPricingRequest request)
    {
        ProductPricingView? productPricingView = await getProductPricing.Handle(new GetProductPricingQuery { Id = id });

        if (productPricingView == null)
        {
            return NotFound();
        }

        GetProductPricingResponse response = mapper.Map<GetProductPricingResponse>(productPricingView);

        var converter = fieldMaskConverterFactory.Create(request.FieldMask);
        JsonSerializerSettings settings = new() { Converters = new List<JsonConverter> { converter } };

        string json = JsonConvert.SerializeObject(response, settings);

        return new OkObjectResult(json) { StatusCode = 200 };
    }
}
