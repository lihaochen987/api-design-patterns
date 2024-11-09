using backend.Database;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class CreateProductController(
    ApplicationDbContext context,
    CreateProductExtensions extensions)
    : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a product", Tags = ["Products"])]
    public async Task<ActionResult<CreateProductResponse>> CreateProduct([FromBody] CreateProductRequest request)
    {
        var product = extensions.ToEntity(request);

        context.Products.Add(product);
        await context.SaveChangesAsync();

        var response = extensions.ToCreateProductResponse(product);
        return CreatedAtAction(
            actionName: "GetProduct",
            controllerName: "GetProduct",
            new { id = product.Id },
            response);
    }
}