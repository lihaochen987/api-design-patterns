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
    ProductFieldMaskConfiguration maskConfiguration,
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

        UpdateBaseProduct(maskConfiguration, request, product);

        switch (product)
        {
            case PetFood petFood:
                UpdatePetFood(maskConfiguration, request, petFood);
                await repository.UpdateProductAsync(product);
                return Ok(mapper.Map<UpdatePetFoodResponse>(product));

            case GroomingAndHygiene groomingAndHygiene:
                UpdateGroomingAndHygiene(maskConfiguration, request, groomingAndHygiene);
                await repository.UpdateProductAsync(product);
                return Ok(mapper.Map<UpdateGroomingAndHygieneResponse>(product));

            default:
                await repository.UpdateProductAsync(product);
                return Ok(mapper.Map<UpdateProductResponse>(product));
        }
    }

    private static void UpdateBaseProduct(
        ProductFieldMaskConfiguration maskConfiguration,
        UpdateProductRequest request,
        DomainModels.Product product)
    {
        (string name, Pricing pricing, Category category, Dimensions dimensions) =
            maskConfiguration.GetUpdatedProductValues(request, product);
        product.Replace(name, pricing, category, dimensions);
    }

    private static void UpdatePetFood(
        ProductFieldMaskConfiguration maskConfiguration,
        UpdateProductRequest request,
        PetFood petFood)
    {
        (AgeGroup ageGroup, BreedSize breedSize, string ingredients, Dictionary<string, object> nutritionalInfo,
                string storageInstructions, decimal weightKg) =
            maskConfiguration.GetUpdatedPetFoodValues(request, petFood);

        petFood.UpdatePetFoodDetails(ageGroup, breedSize, ingredients, nutritionalInfo, storageInstructions,
            weightKg);
    }

    private static void UpdateGroomingAndHygiene(
        ProductFieldMaskConfiguration maskConfiguration,
        UpdateProductRequest request,
        GroomingAndHygiene groomingAndHygiene)
    {
        (bool isNatural, bool isHypoAllergenic, string usageInstructions, bool isCrueltyFree,
                string safetyWarnings) =
            maskConfiguration.GetUpdatedGroomingAndHygieneValues(request, groomingAndHygiene);

        groomingAndHygiene.UpdateGroomingAndHygieneDetails(isNatural, isHypoAllergenic, usageInstructions,
            isCrueltyFree,
            safetyWarnings);
    }
}
