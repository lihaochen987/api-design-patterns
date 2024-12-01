using backend.Product.FieldMasks;
using backend.Product.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductPricingControllers;

[ApiController]
[Route("product")]
public class UpdateProductPricingController(
    IProductRepository productRepository,
    ProductPricingFieldMaskConfiguration configuration,
    UpdateProductPricingExtensions extensions)
    : ControllerBase
{
    [HttpPatch("{id:long}/pricing")]
    [SwaggerOperation(Summary = "Update a product pricing", Tags = ["Products, ProductPricing"])]
    public async Task<ActionResult<UpdateProductPricingResponse>> UpdateProductPricing(
        [FromRoute] long id,
        [FromBody] UpdateProductPricingRequest request)
    {
        DomainModels.Product? productPricing = await productRepository.GetProductAsync(id);

        if (productPricing == null)
        {
            return NotFound();
        }

        (decimal basePrice, decimal discountPercentage, decimal taxRate) =
            configuration.GetUpdatedProductPricingValues(request, productPricing.Pricing);
        productPricing.UpdatePricing(basePrice, discountPercentage, taxRate);

        await productRepository.UpdateProductAsync(productPricing);

        return Ok(extensions.ToUpdateProductPricingResponse(productPricing.Pricing, productPricing.Id));
    }
}
