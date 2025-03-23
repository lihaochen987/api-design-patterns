using AutoMapper;
using backend.Product.ApplicationLayer.Commands.CreateProduct;
using backend.Product.ApplicationLayer.Queries.MapCreateProductResponse;
using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class CreateProductController(
    ICommandHandler<CreateProductCommand> createProduct,
    IQueryHandler<MapCreateProductResponseQuery, CreateProductResponse> createProductResponse,
    IMapper mapper)
    : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a product", Tags = ["Products"])]
    [ProducesResponseType(typeof(CreateProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreateProductResponse>> CreateProduct([FromBody] CreateProductRequest request)
    {
        DomainModels.Product product = request.Category switch
        {
            nameof(Category.PetFood) => mapper.Map<PetFood>(request),
            nameof(Category.GroomingAndHygiene) => mapper.Map<GroomingAndHygiene>(request),
            _ => mapper.Map<DomainModels.Product>(request)
        };
        await createProduct.Handle(new CreateProductCommand { Product = product });

        var response = await createProductResponse.Handle(new MapCreateProductResponseQuery { Product = product });

        return CreatedAtAction(
            "GetProduct",
            "GetProduct",
            new { id = product.Id },
            response);
    }
}
