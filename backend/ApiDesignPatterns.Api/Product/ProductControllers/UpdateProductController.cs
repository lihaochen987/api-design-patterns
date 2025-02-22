using AutoMapper;
using backend.Product.ApplicationLayer.Commands.UpdateProduct;
using backend.Product.ApplicationLayer.Queries.GetProduct;
using backend.Product.DomainModels;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class UpdateProductController(
    IQueryHandler<GetProductQuery, DomainModels.Product> getProduct,
    ICommandHandler<UpdateProductCommand> updateProduct,
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
        DomainModels.Product? product =
            await getProduct.Handle(new GetProductQuery { Id = id });

        if (product == null)
        {
            return NotFound();
        }

        await updateProduct.Handle(new UpdateProductCommand { Request = request, Product = product });

        DomainModels.Product? updatedProduct = await getProduct.Handle(new GetProductQuery { Id = id });

        return updatedProduct switch
        {
            PetFood => Ok(mapper.Map<UpdatePetFoodResponse>(updatedProduct)),
            GroomingAndHygiene => Ok(mapper.Map<UpdateGroomingAndHygieneResponse>(updatedProduct)),
            _ => Ok(mapper.Map<UpdateProductResponse>(updatedProduct))
        };
    }
}
