using backend.Product.Database;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductPricingControllers;

[ApiController]
[Route("product")]
public class UpdateProductPricingController(
    ProductDbContext context,
    ProductFieldMaskConfiguration configuration,
    UpdateProductPricingExtensions extensions) : ControllerBase
{
    [HttpPatch("{id:long}/pricing")]
    [SwaggerOperation(Summary = "Update a product", Tags = ["Products"])]
    public async Task<ActionResult<UpdateProductPricingResponse>> UpdateProductPricing(
        [FromRoute] long id,
        [FromBody] UpdateProductPricingRequest request)
    {
        var productPricing = await context.ProductPricing.FindAsync(id);

        if (productPricing == null)
        {
            return NotFound();
        }

        var (basePrice, discountPercentage, taxRate) =
            configuration.GetUpdatedProductPricingValues(request, productPricing);
        productPricing.Replace(basePrice, discountPercentage, taxRate);

        await context.SaveChangesAsync();

        return Ok(extensions.ToUpdateProductPricingResponse(productPricing));
    }
}