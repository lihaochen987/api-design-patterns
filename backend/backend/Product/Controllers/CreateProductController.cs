using backend.Database;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.Controllers;

[ApiController]
[Route("product")]
public class CreateProductController(ApplicationDbContext context) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a product", Tags = ["Products"])]
    public async Task<ActionResult<CreateProductResponse>> CreateProduct([FromBody] CreateProductRequest request)
    {
        var product = request.ToEntity();

        context.Products.Add(product);
        await context.SaveChangesAsync();

        var response = product.ToCreateProductResponse();
        return CreatedAtAction(
            actionName: "GetProduct",
            controllerName: "GetProduct",
            new { id = product.Id },
            response);
    }
}