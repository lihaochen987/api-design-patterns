// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels.ValueObjects;
using backend.Product.ProductControllers;

namespace backend.Product.Services.ProductServices;

public class DimensionsFieldMaskService
{
    public Dimensions GetUpdatedDimensionValues(
        UpdateProductRequest request,
        Dimensions currentDimensions)
    {
        decimal length = request.FieldMask.Contains("dimensions.length", StringComparer.OrdinalIgnoreCase)
                         && !string.IsNullOrEmpty(request.Dimensions?.Length)
            ? decimal.Parse(request.Dimensions.Length)
            : currentDimensions.Length;

        decimal width = request.FieldMask.Contains("dimensions.width", StringComparer.OrdinalIgnoreCase)
                        && !string.IsNullOrEmpty(request.Dimensions?.Width)
            ? decimal.Parse(request.Dimensions.Width)
            : currentDimensions.Width;

        decimal height = request.FieldMask.Contains("dimensions.height", StringComparer.OrdinalIgnoreCase)
                         && !string.IsNullOrEmpty(request.Dimensions?.Height)
            ? decimal.Parse(request.Dimensions.Height)
            : currentDimensions.Height;

        return new Dimensions(length, width, height);
    }
}
