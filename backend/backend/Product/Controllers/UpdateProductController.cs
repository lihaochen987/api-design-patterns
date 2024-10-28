using System.Reflection;
using System.Text.RegularExpressions;
using backend.Database;
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

        // var fieldMaskSerializer = new FieldMaskSerializer();
        // var validFields = fieldMaskSerializer.GetValidFields(request.FieldMask, product);
        //
        // // Apply each valid field update to the product
        // foreach (var field in validFields)
        // {
        //     var productProperty = GetNestedProperty(product, field);
        //     var requestProperty = GetNestedProperty(request, field);
        //
        //     if (productProperty == null || requestProperty == null) continue;
        //     var newValue = requestProperty.GetValue(request);
        //     if (newValue == null) continue;
        //
        //     // Handle type conversion for compatible types, e.g., string to decimal
        //     if (productProperty.PropertyType != requestProperty.PropertyType)
        //     {
        //         var convertedValue = ConvertToPropertyType(newValue, productProperty.PropertyType);
        //         if (convertedValue != null)
        //         {
        //             productProperty.SetValue(product, convertedValue);
        //         }
        //     }
        //     else
        //     {
        //         productProperty.SetValue(product, newValue);
        //     }
        // }

        await context.SaveChangesAsync();

        return Ok(product.ToUpdateProductResponse());
    }
    
    private static PropertyInfo? GetNestedProperty(object obj, string field)
    {
        var parts = field.Split('.');
        Type? type = obj.GetType();
        PropertyInfo? property = null;

        foreach (var part in parts)
        {
            property = type?.GetProperty(part, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (property == null) return null;
            type = property.PropertyType;
        }

        return property;
    }

    private static object? GetNestedObject(object obj, string field)
    {
        var parts = field.Split('.');
        foreach (var part in parts.SkipLast(1))
        {
            var property = obj.GetType().GetProperty(part, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (property == null) return null;
            obj = property.GetValue(obj);
        }
        return obj;
    }

    private static object? ConvertToPropertyType(object value, Type targetType)
    {
        try
        {
            // Convert string to decimal or other types if needed
            if (targetType == typeof(decimal) && value is string stringValue &&
                decimal.TryParse(stringValue, out var decimalValue))
            {
                return decimalValue;
            }

            if (targetType.IsEnum && value is string enumStringValue)
            {
                return Enum.Parse(targetType, enumStringValue, ignoreCase: true);
            }

            // Convert to target type using ChangeType for compatible types
            return Convert.ChangeType(value, targetType);
        }
        catch
        {
            // Handle or log any conversion errors as needed
            return null;
        }
    }

    private bool IsComplexType(Type type)
    {
        return type.IsClass && type != typeof(string);
    }

    [GeneratedRegex(@"[^\d.-]")]
    private static partial Regex MyRegex();
}