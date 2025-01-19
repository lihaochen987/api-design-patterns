using AutoMapper;
using backend.Product.ApplicationLayer;
using backend.Product.ApplicationLayer.GetProduct;
using backend.Product.ApplicationLayer.UpdateProduct;
using backend.Product.DomainModels;
using backend.Shared;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class UpdateProductController(
    IQueryHandler<GetProductQuery, DomainModels.Product> getProduct,
    ICommandHandler<UpdateProductQuery> updateProduct,
    IMapper mapper)
    : ControllerBase
{
    [HttpPatch("{id:long}")]
    [SwaggerOperation(Summary = "Update a product", Tags = ["Products"])]
    [ProducesResponseType(typeof(UpdateProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateProductResponse>> UpdateProduct(
        [FromRoute] long id,
        [FromBody] UpdateProductRequest request)
    {
        DomainModels.Product? product = await getProduct.Handle(new GetProductQuery { Id = id });

        if (product == null)
        {
            return NotFound();
        }

        await updateProduct.Handle(new UpdateProductQuery { Request = request, Product = product });

        return product switch
        {
            PetFood => Ok(mapper.Map<UpdatePetFoodResponse>(product)),
            GroomingAndHygiene => Ok(mapper.Map<UpdateGroomingAndHygieneResponse>(product)),
            _ => Ok(mapper.Map<UpdateProductResponse>(product))
        };
    }
}
