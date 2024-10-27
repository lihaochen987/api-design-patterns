using backend.Database;
using backend.Shared;
using backend.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace backend.Product.Controllers;

[ApiController]
[Route("product")]
public class GetProductController(
    ApplicationDbContext context,
    ISerializationService serializationService)
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

        var contractResolver = new DynamicContractResolver(request.FieldMask, product, serializationService);
        var jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = contractResolver,
            Formatting = Formatting.Indented
        };

        var json = JsonConvert.SerializeObject(response, jsonSettings);
        return new OkObjectResult(json)
        {
            StatusCode = 200
        };
    }
}