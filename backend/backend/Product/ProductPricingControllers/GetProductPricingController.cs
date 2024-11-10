using backend.Product.Database;
using backend.Product.FieldMasks;
using backend.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductPricingControllers;

[ApiController]
[Route("product")]
public class GetProductPricingController(
    ProductDbContext context,
    ProductFieldMaskConfiguration configuration,
    GetProductPricingExtensions extensions)
    : ControllerBase
{
    [HttpGet("{id:long}/pricing")]
    [SwaggerOperation(Summary = "Get a product pricing", Tags = ["Products, ProductPricing"])]
    public async Task<ActionResult<GetProductPricingResponse>> GetProductPricing(
        [FromRoute] long id,
        [FromQuery] GetProductPricingRequest request)
    {
        var productPricing = await context.ProductPricing.FindAsync(id);
        if (productPricing == null) return NotFound();

        var response = extensions.ToGetProductPricingResponse(productPricing);

        var settings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter>
                { new FieldMaskConverter(request.FieldMask, configuration.ProductPricingFieldPaths) }
        };

        var json = JsonConvert.SerializeObject(response, settings);

        return new OkObjectResult(json)
        {
            StatusCode = 200
        };
    }
}