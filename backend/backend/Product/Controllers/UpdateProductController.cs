using System.Reflection;
using System.Text.RegularExpressions;
using backend.Database;
using backend.Shared.FieldMasks;
using Microsoft.AspNetCore.Mvc;

namespace backend.Product.Controllers;

[ApiController]
[Route("product")]
public partial class UpdateProductController(ApplicationDbContext context) : ControllerBase
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

        ApplyFieldMaskUpdates(
            request,
            product,
            request.FieldMask,
            new FieldMaskSerializer());

        await context.SaveChangesAsync();

        return Ok(product.ToUpdateProductResponse());
    }

    private void ApplyFieldMaskUpdates(
        object source,
        object target,
        List<string> fieldMask,
        FieldMaskSerializer fieldMaskSerializer)
    {
        var validFields = fieldMaskSerializer.GetValidFields(fieldMask, target);

        ApplyProperties(source, target, validFields, fieldMaskSerializer);
    }

    private void ApplyProperties(object source, object target, HashSet<string> validFields,
        FieldMaskSerializer fieldMaskSerializer, string? prefix = null)
    {
        var entityType = target.GetType();

        foreach (var property in entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var propertyPath = prefix == null ? property.Name.ToLower() : $"{prefix}.{property.Name.ToLower()}";

            if (!validFields.Contains(propertyPath)) continue;

            var sourceProperty = source.GetType().GetProperty(property.Name);
            if (sourceProperty == null) continue;

            var newValue = sourceProperty.GetValue(source);

            if (newValue != null && fieldMaskSerializer.IsJsonPropertySerializable(property, validFields, target))
            {
                if (IsComplexType(property.PropertyType))
                {
                    var targetNestedProperty =
                        property.GetValue(target) ?? Activator.CreateInstance(property.PropertyType);
                    property.SetValue(target, targetNestedProperty);
                    if (targetNestedProperty != null)
                        ApplyProperties(newValue, targetNestedProperty, validFields, fieldMaskSerializer, propertyPath);
                }
                else
                {
                    var convertedValue = Convert.ChangeType(newValue, property.PropertyType);
                    property.SetValue(target, convertedValue);
                }
            }
        }
    }

    private bool IsComplexType(Type type)
    {
        return type.IsClass && type != typeof(string);
    }

    [GeneratedRegex(@"[^\d.-]")]
    private static partial Regex MyRegex();
}