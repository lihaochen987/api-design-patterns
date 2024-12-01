using AutoMapper;
using backend.Product.DomainModels.Enums;
using backend.Product.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class ReplaceProductController(
    IProductRepository productRepository,
    ReplaceProductExtensions extensions,
    IMapper mapper)
    : ControllerBase
{
    [HttpPut("{id:long}")]
    [SwaggerOperation(Summary = "Replace a product", Tags = ["Products"])]
    public async Task<ActionResult<ReplaceProductResponse>> ReplaceProduct(
        [FromRoute] long id,
        [FromBody] ReplaceProductRequest request)
    {
        DomainModels.Product product = extensions.ToEntity(request);

        DomainModels.Product? existingProduct = await productRepository.GetProductAsync(id);
        if (existingProduct == null)
        {
            return NotFound();
        }

        await productRepository.ReplaceProductAsync(existingProduct);
        object response = product.Category switch
        {
            Category.PetFood => mapper.Map<ReplacePetFoodResponse>(product),
            Category.GroomingAndHygiene => mapper.Map<ReplaceGroomingAndHygieneResponse>(product),
            _ => mapper.Map<ReplacePetFoodResponse>(product)
        };

        return Ok(response);
    }
}
