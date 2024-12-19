using backend.Product.DomainModels.Views;
using backend.Product.InfrastructureLayer;
using backend.Product.Services;
using backend.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductPricingControllers;

[ApiController]
[Route("product")]
public class GetProductPricingController(
    IProductPricingRepository productRepository,
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
        ProductPricingView? product = await productRepository.GetProductPricingAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        GetProductPricingResponse response = extensions.ToGetProductPricingResponse(product.Pricing, product.Id);

        JsonSerializerSettings settings = new()
        {
            Converters = new List<JsonConverter>
            {
                new FieldMaskConverter(request.FieldMask, configuration.ProductPricingFieldPaths)
            }
        };

        string json = JsonConvert.SerializeObject(response, settings);

        return new OkObjectResult(json) { StatusCode = 200 };
    }
}
