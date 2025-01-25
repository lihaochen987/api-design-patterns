using backend.Product.ApplicationLayer.Queries.GetProductPricing;
using backend.Product.DomainModels.Views;
using backend.Product.Services.ProductPricingServices;
using backend.Shared.FieldMask;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductPricingControllers;

[ApiController]
[Route("product")]
public class GetProductPricingController(
    IQueryHandler<GetProductPricingQuery, ProductPricingView> getProductPricing,
    ProductPricingFieldPaths fieldPaths,
    GetProductPricingExtensions extensions,
    IFieldMaskConverterFactory fieldMaskConverterFactory)
    : ControllerBase
{
    [HttpGet("{id:long}/pricing")]
    [SwaggerOperation(Summary = "Get a product pricing", Tags = ["Products, ProductPricing"])]
    public async Task<ActionResult<GetProductPricingResponse>> GetProductPricing(
        [FromRoute] long id,
        [FromQuery] GetProductPricingRequest request)
    {
        ProductPricingView? product = await getProductPricing.Handle(new GetProductPricingQuery { Id = id });

        if (product == null)
        {
            return NotFound();
        }

        GetProductPricingResponse response = extensions.ToGetProductPricingResponse(product.Pricing, product.Id);

        var converter = fieldMaskConverterFactory.Create(request.FieldMask, fieldPaths.ValidPaths);
        JsonSerializerSettings settings = new() { Converters = new List<JsonConverter> { converter } };

        string json = JsonConvert.SerializeObject(response, settings);

        return new OkObjectResult(json) { StatusCode = 200 };
    }
}
