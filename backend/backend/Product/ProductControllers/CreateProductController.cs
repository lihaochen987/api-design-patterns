using backend.Product.Database;
using backend.Product.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class CreateProductController(
    IProductRepository productRepository,
    CreateProductExtensions extensions)
    : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a product", Tags = ["Products"])]
    public async Task<ActionResult<CreateProductResponse>> CreateProduct([FromBody] CreateProductRequest request)
    {
        var product = extensions.ToEntity(request);
        await productRepository.CreateProductAsync(product);

        var response = extensions.ToCreateProductResponse(product);
        return CreatedAtAction(
            actionName: "GetProduct",
            controllerName: "GetProduct",
            new { id = product.Id },
            response);
    }
}