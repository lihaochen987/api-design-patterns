using AutoMapper;
using backend.Product.ApplicationLayer;
using backend.Product.DomainModels.Enums;
using backend.Product.InfrastructureLayer;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class DeleteProductController(
    IProductApplicationService applicationService,
    IMapper mapper)
    : ControllerBase
{
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Delete a product", Tags = ["Products"])]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteProduct(
        [FromRoute] long id,
        [FromQuery] DeleteProductRequest request)
    {
        DomainModels.Product? product = await applicationService.GetProductAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        GetProductResponse response = product.Category switch
        {
            Category.PetFood => mapper.Map<GetPetFoodResponse>(product),
            Category.GroomingAndHygiene => mapper.Map<GetGroomingAndHygieneResponse>(product),
            _ => mapper.Map<GetProductResponse>(product)
        };

        await applicationService.DeleteProductAsync(product);
        return NoContent();
    }
}
