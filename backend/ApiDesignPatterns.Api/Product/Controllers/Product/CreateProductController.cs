using backend.Product.ApplicationLayer.Commands.CreateProduct;
using backend.Product.ApplicationLayer.Queries.MapCreateProductRequest;
using backend.Product.ApplicationLayer.Queries.MapCreateProductResponse;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.Controllers.Product;

[ApiController]
[Route("product")]
public class CreateProductController(
    ICommandHandler<CreateProductCommand> createProduct,
    IQueryHandler<MapCreateProductResponseQuery, CreateProductResponse> createProductResponse,
    IQueryHandler<MapCreateProductRequestQuery, DomainModels.Product> mapProductRequest)
    : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a product", Tags = ["Products"])]
    [ProducesResponseType(typeof(CreateProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreateProductResponse>> CreateProduct([FromBody] CreateProductRequest request)
    {
        var product = await mapProductRequest.Handle(new MapCreateProductRequestQuery { Request = request });

        await createProduct.Handle(new CreateProductCommand { Product = product });

        var response = await createProductResponse.Handle(new MapCreateProductResponseQuery { Product = product });

        return CreatedAtAction(
            "GetProduct",
            "GetProduct",
            new { id = product.Id },
            response);
    }
}
