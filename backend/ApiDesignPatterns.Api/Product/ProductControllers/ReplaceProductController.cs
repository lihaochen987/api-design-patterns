using AutoMapper;
using backend.Product.ApplicationLayer;
using backend.Product.ApplicationLayer.ReplaceProduct;
using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Shared;
using backend.Shared.CommandService;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class ReplaceProductController(
    IProductQueryApplicationService productQueryApplicationService,
    ICommandService<ReplaceProduct> service,
    IMapper mapper)
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
        DomainModels.Product? existingProduct = await productQueryApplicationService.GetProductAsync(id);
        if (existingProduct == null)
        {
            return NotFound();
        }

        switch (existingProduct)
        {
            case PetFood petFood:
                mapper.Map(request, petFood);
                break;

            case GroomingAndHygiene groomingAndHygiene:
                mapper.Map(request, groomingAndHygiene);
                break;

            default:
                mapper.Map(request, existingProduct);
                break;
        }

        await service.Execute(new ReplaceProduct{Product = existingProduct});
        object response = existingProduct.Category switch
        {
            Category.PetFood => mapper.Map<ReplacePetFoodResponse>(existingProduct),
            Category.GroomingAndHygiene => mapper.Map<ReplaceGroomingAndHygieneResponse>(existingProduct),
            _ => mapper.Map<ReplaceProductResponse>(existingProduct)
        };

        return Ok(response);
    }
}
