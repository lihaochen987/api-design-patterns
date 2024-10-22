using backend.Database;
using Microsoft.AspNetCore.Mvc;

namespace backend.Product.Controllers;

[ApiController]
[Route("/product")]
public class CreateProductController(ApplicationDbContext context) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateProductResponse>> CreateProduct([FromBody] CreateProductRequest request)
    {
        var product = ProductMapper.MapToDomain(request);
        
        context.Products.Add(product);
        await context.SaveChangesAsync();

        var response = ProductMapper.MapToCreateProductResponse(product);
        return CreatedAtAction(
            actionName: "GetProduct",
            controllerName: "GetProduct",
            new { id = product.Id },
            response);
    }
}