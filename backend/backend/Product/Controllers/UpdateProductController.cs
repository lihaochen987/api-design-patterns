using backend.Database;
using backend.Product.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Product.Controllers;

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

        var (name, price, category, dimensions) = configuration.GetUpdatedProductValues(request, product);
        product.Replace(name, price, category, dimensions);

        await context.SaveChangesAsync();

        return Ok(extensions.ToUpdateProductResponse(product));
    }
}