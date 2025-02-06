using AutoMapper;
using backend.Product.ApplicationLayer.Commands.ReplaceProduct;
using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.ApplicationLayer.Queries.ReplaceProductResponse;
using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class ReplaceProductController(
    IQueryHandler<GetProductQuery, DomainModels.Product> getProduct,
    ICommandHandler<ReplaceProductCommand> replaceProduct,
    IQueryHandler<ReplaceProductResponseQuery, ReplaceProductResponse> replaceProductResponse)
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

        await replaceProduct.Handle(new ReplaceProductCommand { Product = existingProduct, Request = request });
        var response =
            await replaceProductResponse.Handle(new ReplaceProductResponseQuery { Product = existingProduct });

        return Ok(response);
    }
}
