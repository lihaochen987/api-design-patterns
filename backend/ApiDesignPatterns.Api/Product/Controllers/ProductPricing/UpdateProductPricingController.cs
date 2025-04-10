using AutoMapper;
using backend.Product.ApplicationLayer.Commands.UpdateProductPricing;
using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.Controllers.ProductPricing;

[ApiController]
[Route("product")]
public class UpdateProductPricingController(
    IQueryHandler<GetProductQuery, DomainModels.Product?> getProduct,
    ICommandHandler<UpdateProductPricingCommand> updateProductPricing,
    IMapper mapper)
    : ControllerBase
{
    [HttpPatch("{id:long}/pricing")]
    [SwaggerOperation(Summary = "Update a product pricing", Tags = ["Products, ProductPricing"])]
    public async Task<ActionResult<UpdateProductPricingResponse>> UpdateProductPricing(
        [FromRoute] long id,
        [FromBody] UpdateProductPricingRequest request)
    {
        DomainModels.Product? product = await getProduct.Handle(new GetProductQuery { Id = id });

        if (product == null)
        {
            return NotFound();
        }

        await updateProductPricing.Handle(
            new UpdateProductPricingCommand { Product = product, Request = request });
        var response = mapper.Map<UpdateProductPricingResponse>(product);

        return Ok(response);
    }
}
