using AutoMapper;
using backend.Product.ApplicationLayer.Commands.CreateProduct;
using backend.Product.DomainModels.Enums;
using backend.Shared.CommandHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class CreateProductController(
    ICommandHandler<CreateProductQuery> createProduct,
    CreateProductExtensions extensions,
    IMapper mapper)
    : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a product", Tags = ["Products"])]
    [ProducesResponseType(typeof(CreateProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreateProductResponse>> CreateProduct([FromBody] CreateProductRequest request)
    {
        DomainModels.Product product = extensions.ToEntity(request);
        await createProduct.Handle(new CreateProductQuery { Product = product });

        object response = product.Category switch
        {
            Category.PetFood => mapper.Map<CreatePetFoodResponse>(product),
            Category.GroomingAndHygiene => mapper.Map<CreateGroomingAndHygieneResponse>(product),
            _ => mapper.Map<CreateProductResponse>(product)
        };

        return CreatedAtAction(
            "GetProduct",
            "GetProduct",
            new { id = product.Id },
            response);
    }
}
