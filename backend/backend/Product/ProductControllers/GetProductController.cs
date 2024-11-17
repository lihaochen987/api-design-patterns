using backend.Product.Database;
using backend.Product.FieldMasks;
using backend.Product.ViewModels;
using backend.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class GetProductController(
    ProductDbContext context,
    ProductFieldMaskConfiguration configuration,
    GetProductExtensions extensions)
    : ControllerBase
{
    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Get a product", Tags = ["Products"])]
    public async Task<ActionResult<GetProductResponse>> GetProduct(
        [FromRoute] long id,
        [FromQuery] GetProductRequest request)
    {
        var product = await context.Set<ProductView>()
            .FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return NotFound();

        var response = extensions.ToGetProductResponse(product);

        var settings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter>
                { new FieldMaskConverter(request.FieldMask, configuration.ProductFieldPaths) }
        };

        var json = JsonConvert.SerializeObject(response, settings);

        return new OkObjectResult(json)
        {
            StatusCode = 200
        };
    }
}