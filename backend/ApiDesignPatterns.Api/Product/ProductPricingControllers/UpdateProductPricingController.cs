using backend.Product.InfrastructureLayer;
using backend.Product.InfrastructureLayer.Database.Product;
using backend.Product.Services.ProductPricingServices;
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
        DomainModels.Product? product = await productRepository.GetProductAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        (decimal basePrice, decimal discountPercentage, decimal taxRate) =
            configuration.GetUpdatedProductPricingValues(request, product.Pricing);

        var updatedPricing = product.Pricing with
        {
            BasePrice = basePrice,
            DiscountPercentage = discountPercentage,
            TaxRate = taxRate
        };
        product.Pricing = updatedPricing;

        await productRepository.UpdateProductAsync(product);
        return Ok(extensions.ToUpdateProductPricingResponse(product.Pricing, product.Id));
    }
}
