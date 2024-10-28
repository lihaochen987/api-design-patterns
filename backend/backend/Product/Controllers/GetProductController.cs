using backend.Database;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace backend.Product.Controllers;

[ApiController]
[Route("product")]
public class GetProductController(
    ApplicationDbContext context)
    : ControllerBase
{
    [HttpGet("{id:long}")]
    public async Task<ActionResult<GetProductResponse>> GetProduct(
        [FromRoute] long id,
        [FromQuery] GetProductRequest request)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null) return NotFound();

        var response = product.ToGetProductResponse();

        var settings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new ProductFieldMaskConverter(request.FieldMask) }
        };

        var json = JsonConvert.SerializeObject(response, settings);

        return new OkObjectResult(json)
        {
            StatusCode = 200
        };
    }
}