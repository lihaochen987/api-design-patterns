using backend.Database;
using backend.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.Controllers;

[ApiController]
[Route("product")]
public class GetProductController(
    ApplicationDbContext context,
    ProductFieldMaskConfiguration configuration)
    : ControllerBase
{
    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Get a product", Tags = ["Products"])]
    public async Task<ActionResult<GetProductResponse>> GetProduct(
        [FromRoute] long id,
        [FromQuery] GetProductRequest request)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null) return NotFound();

        var response = product.ToGetProductResponse();

        var settings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter>
                { new FieldMaskConverter(request.FieldMask, configuration.AllFieldPaths) }
        };

        var json = JsonConvert.SerializeObject(response, settings);

        return new OkObjectResult(json)
        {
            StatusCode = 200
        };
    }
}