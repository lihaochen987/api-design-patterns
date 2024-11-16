using backend.Product.Database;
using backend.Product.FieldMasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductPricingControllers;

[ApiController]
[Route("product")]
public class UpdateProductPricingController(
    ProductDbContext context,
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
        var product = await context.Products
            .Include(p => p.Pricing)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        var (basePrice, discountPercentage, taxRate) =
            configuration.GetUpdatedProductPricingValues(request, product.Pricing);
        product.UpdatePricing(basePrice, discountPercentage, taxRate);

        await context.SaveChangesAsync();

        return Ok(extensions.ToUpdateProductPricingResponse(product.Pricing, product.Id));
    }
}