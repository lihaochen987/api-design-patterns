using backend.Product.Database;
using backend.Product.FieldMasks;
using backend.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductPricingControllers;

[ApiController]
[Route("product")]
public class GetProductPricingController(
    ProductDbContext context,
    ProductPricingFieldMaskConfiguration configuration,
    GetProductPricingExtensions extensions)
    : ControllerBase
{
    [HttpGet("{id:long}/pricing")]
    [SwaggerOperation(Summary = "Get a product pricing", Tags = ["Products, ProductPricing"])]
    public async Task<ActionResult<GetProductPricingResponse>> GetProductPricing(
        [FromRoute] long id,
        [FromQuery] GetProductPricingRequest request)
    {
        var productPricing = await context.Products
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => p.Pricing)
            .FirstOrDefaultAsync();
        
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