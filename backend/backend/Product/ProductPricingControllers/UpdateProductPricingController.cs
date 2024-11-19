using backend.Product.Database;
using backend.Product.FieldMasks;
using backend.Product.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        var productPricing = await productRepository.GetProductAsync(id);

        if (productPricing == null)
        {
            return NotFound();
        }

        var (basePrice, discountPercentage, taxRate) =
            configuration.GetUpdatedProductPricingValues(request, productPricing.Pricing);
        productPricing.UpdatePricing(basePrice, discountPercentage, taxRate);

        await productRepository.ReplaceProductAsync(productPricing);

        return Ok(extensions.ToUpdateProductPricingResponse(productPricing.Pricing, productPricing.Id));
    }
}