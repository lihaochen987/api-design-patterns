using backend.Database;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.ProductControllers;

[ApiController]
[Route("product")]
public class UpdateProductController(
    ApplicationDbContext context,
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
        var product = await context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        var (name, basePrice, discountPercentage, taxRate, category, dimensions) =
            configuration.GetUpdatedProductValues(request, product);
        product.Replace(name, basePrice, discountPercentage, taxRate, category, dimensions);

        await context.SaveChangesAsync();

        return Ok(extensions.ToUpdateProductResponse(product));
    }
}