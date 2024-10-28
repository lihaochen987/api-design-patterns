using backend.Database;
using backend.Product.DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace backend.Product.Controllers;

[ApiController]
[Route("product")]
public class UpdateProductController(ApplicationDbContext context) : ControllerBase
{
    // Todo: Actually implement partial Updates because it's hard
    [HttpPatch("{id:long}")]
    public async Task<ActionResult<UpdateProductResponse>> UpdateProduct(
        [FromRoute] long id,
        [FromBody] UpdateProductRequest request)
    {
        var product = await context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        var (name, price, category, dimensions) = GetUpdatedProductValues(request, product);
        product.Replace(name, price, category, dimensions);

        await context.SaveChangesAsync();

        return Ok(product.ToUpdateProductResponse());
    }

    private static (
        string name,
        decimal price,
        Category category,
        Dimensions dimensions)
        GetUpdatedProductValues(
            UpdateProductRequest request,
            DomainModels.Product product)
    {
        var name = request.FieldMask.Contains("name", StringComparer.OrdinalIgnoreCase)
                   && !string.IsNullOrEmpty(request.Name)
            ? request.Name
            : product.Name;

        var price = request.FieldMask.Contains("price", StringComparer.OrdinalIgnoreCase)
                    && decimal.TryParse(request.Price, out var parsedPrice)
            ? parsedPrice
            : product.Price;

        var category = request.FieldMask.Contains("category", StringComparer.OrdinalIgnoreCase)
                       && Enum.TryParse(request.Category, true, out Category parsedCategory)
            ? parsedCategory
            : product.Category;

        var dimensions = GetUpdatedDimensionValues(request, product.Dimensions);

        return (name, price, category, dimensions);
    }

    private static Dimensions GetUpdatedDimensionValues(
        UpdateProductRequest request,
        Dimensions currentDimensions)
    {
        var length = request.FieldMask.Contains("dimensions.length", StringComparer.OrdinalIgnoreCase)
                     && !string.IsNullOrEmpty(request.Dimensions.Length)
            ? decimal.Parse(request.Dimensions.Length)
            : currentDimensions.Length;

        var width = request.FieldMask.Contains("dimensions.width", StringComparer.OrdinalIgnoreCase)
                    && !string.IsNullOrEmpty(request.Dimensions.Width)
            ? decimal.Parse(request.Dimensions.Width)
            : currentDimensions.Width;

        var height = request.FieldMask.Contains("dimensions.height", StringComparer.OrdinalIgnoreCase)
                     && !string.IsNullOrEmpty(request.Dimensions.Height)
            ? decimal.Parse(request.Dimensions.Height)
            : currentDimensions.Height;

        return new Dimensions(length, width, height);
    }
}