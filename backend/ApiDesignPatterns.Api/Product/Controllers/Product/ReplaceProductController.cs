using backend.Product.ApplicationLayer.Commands.ReplaceProduct;
using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.Services.Mappers;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.Controllers.Product;

[ApiController]
[Route("product")]
public class ReplaceProductController(
    IAsyncQueryHandler<GetProductQuery, DomainModels.Product?> getProduct,
    ICommandHandler<ReplaceProductCommand> replaceProduct,
    IProductTypeMapper productTypeMapper)
    : ControllerBase
{
    [HttpPut("{id:long}")]
    [SwaggerOperation(Summary = "Replace a product", Tags = ["Products"])]
    [ProducesResponseType(typeof(ReplaceProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReplaceProductResponse>> ReplaceProduct(
        [FromRoute] long id,
        [FromBody] ReplaceProductRequest request)
    {
        DomainModels.Product? existingProduct = await getProduct.Handle(new GetProductQuery { Id = id });
        if (existingProduct == null)
        {
            return NotFound();
        }

        if (existingProduct.Category.ToString() != request.Category)
        {
            return BadRequest();
        }

        await replaceProduct.Handle(new ReplaceProductCommand
        {
            ExistingProductId = existingProduct.Id, Request = request
        });
        var replacedProduct = await getProduct.Handle(new GetProductQuery { Id = id });
        if (replacedProduct == null)
        {
            return NotFound();
        }

        var response =
            productTypeMapper.MapToResponse<ReplaceProductResponse>(replacedProduct, ProductControllerMethod.Replace);

        return Ok(response);
    }
}
