using backend.Product.DomainModels;
using backend.Product.FieldMasks;
using backend.Product.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class UpdateProductController(
    IProductRepository repository,
    ProductFieldMaskConfiguration configuration,
    UpdateProductExtensions extensions)
    : ControllerBase
{
    [HttpPatch("{id:long}")]
    [SwaggerOperation(Summary = "Update a product", Tags = ["Products"])]
    public async Task<ActionResult<UpdateProductResponse>> UpdateProduct(
        [FromRoute] long id,
        [FromBody] UpdateProductRequest request)
    {
        var product = await repository.GetProductAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        var (name, pricing, category, dimensions) =
            configuration.GetUpdatedProductValues(request, product);
        product.Replace(name, pricing, category, dimensions);

        if (product is PetFood petFood)
        {
            var (ageGroup, breedSize, ingredients, nutritionalInfo, storageInstructions, weightKg) =
                configuration.GetUpdatedPetFoodValues(request, petFood);

            petFood.UpdatePetFoodDetails(ageGroup, breedSize, ingredients, nutritionalInfo, storageInstructions,
                weightKg);
        }

        await repository.ReplaceProductAsync(product);

        return Ok(extensions.ToUpdateProductResponse(product));
    }
}