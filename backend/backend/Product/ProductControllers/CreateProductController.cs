using backend.Product.DomainModels.Enums;
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
        DomainModels.Product product = extensions.ToEntity(request);
        await productRepository.CreateProductAsync(product);

        object response = product.Category switch
        {
            Category.PetFood => extensions.ToCreatePetFoodResponse(product),
            Category.GroomingAndHygiene => extensions.ToCreateGroomingAndHygieneResponse(product),
            _ => extensions.ToCreateProductResponse(product)
        };

        return CreatedAtAction(
            "GetProduct",
            "GetProduct",
            new { id = product.Id },
            response);
    }
}
