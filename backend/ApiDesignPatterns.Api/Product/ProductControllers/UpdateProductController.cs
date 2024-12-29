using AutoMapper;
using backend.Product.ApplicationLayer;
using backend.Product.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class UpdateProductController(
    IProductApplicationService applicationService,
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
        DomainModels.Product? product = await applicationService.GetProductAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        await applicationService.UpdateProductAsync(request, product);

        return product switch
        {
            PetFood => Ok(mapper.Map<UpdatePetFoodResponse>(product)),
            GroomingAndHygiene => Ok(mapper.Map<UpdateGroomingAndHygieneResponse>(product)),
            _ => Ok(mapper.Map<UpdateProductResponse>(product))
        };
    }
}
