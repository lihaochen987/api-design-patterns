using AutoMapper;
using backend.Product.DomainModels;
using backend.Product.DomainModels.Enums;
using backend.Product.DomainModels.ValueObjects;
using backend.Product.InfrastructureLayer;
using backend.Product.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class UpdateProductController(
    IProductRepository repository,
    ProductFieldMaskConfiguration configuration,
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
        DomainModels.Product? product = await repository.GetProductAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        (string name, Pricing pricing, Category category, Dimensions dimensions) =
            configuration.GetUpdatedProductValues(request, product);
        product.Replace(name, pricing, category, dimensions);

        switch (product)
        {
            case PetFood petFood:
                (AgeGroup ageGroup, BreedSize breedSize, string ingredients, Dictionary<string, object> nutritionalInfo,
                        string storageInstructions, decimal weightKg) =
                    configuration.GetUpdatedPetFoodValues(request, petFood);

                petFood.UpdatePetFoodDetails(ageGroup, breedSize, ingredients, nutritionalInfo, storageInstructions,
                    weightKg);
                await repository.UpdateProductAsync(product);
                return Ok(mapper.Map<UpdatePetFoodResponse>(product));

            case GroomingAndHygiene groomingAndHygiene:
                (bool isNatural, bool isHypoAllergenic, string usageInstructions, bool isCrueltyFree,
                        string safetyWarnings) =
                    configuration.GetUpdatedGroomingAndHygieneValues(request, groomingAndHygiene);

                groomingAndHygiene.UpdateGroomingAndHygieneDetails(isNatural, isHypoAllergenic, usageInstructions,
                    isCrueltyFree,
                    safetyWarnings);
                await repository.UpdateProductAsync(product);
                return Ok(mapper.Map<UpdateGroomingAndHygieneResponse>(product));

            default:
                await repository.UpdateProductAsync(product);
                return Ok(mapper.Map<UpdateProductResponse>(product));
        }
    }
}
