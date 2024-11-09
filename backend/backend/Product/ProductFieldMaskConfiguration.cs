using backend.Product.DomainModels;
using backend.Product.ProductControllers;

namespace backend.Product;

using System.Collections.Generic;

/// <summary>
/// ProductMaskFieldPaths is a class which holds all the manual changes required for this API to work.
/// For instance when adding a new field or value object you would have to:
/// 1. Add the new object to the AllFieldPaths (partial retrievals)
/// 2. Add parsing logic for GetUpdatedProductValues (partial updates)
/// 3. Add the mapping in the extension methods (TBC on making this more generic and easier)
/// </summary>
public class ProductFieldMaskConfiguration
{
    public readonly HashSet<string> ProductFieldPaths =
    [
        "*",
        "id",
        "name",
        "price",
        "category",
        "dimensions.*",
        "dimensions.width",
        "dimensions.height",
        "dimensions.length"
    ];

    public (
        string name,
        decimal basePrice,
        decimal discountPercentage,
        decimal taxRate,
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

        var basePrice = request.FieldMask.Contains("price", StringComparer.OrdinalIgnoreCase)
                        && decimal.TryParse(request.BasePrice, out var parsedBasePrice)
            ? parsedBasePrice
            : product.BasePrice;

        var discountPercentage = request.FieldMask.Contains("price", StringComparer.OrdinalIgnoreCase)
                                 && decimal.TryParse(request.BasePrice, out var parsedDiscountPercentage)
            ? parsedDiscountPercentage
            : product.BasePrice;

        var taxRate = request.FieldMask.Contains("price", StringComparer.OrdinalIgnoreCase)
                      && decimal.TryParse(request.BasePrice, out var parsedTaxRate)
            ? parsedTaxRate
            : product.BasePrice;

        var category = request.FieldMask.Contains("category", StringComparer.OrdinalIgnoreCase)
                       && Enum.TryParse(request.Category, true, out Category parsedCategory)
            ? parsedCategory
            : product.Category;

        var dimensions = GetUpdatedDimensionValues(request, product.Dimensions);

        return (name, basePrice, discountPercentage, taxRate, category, dimensions);
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