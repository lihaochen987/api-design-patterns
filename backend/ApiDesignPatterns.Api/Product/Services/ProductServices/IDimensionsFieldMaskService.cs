// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels.ValueObjects;
using backend.Product.ProductControllers;

namespace backend.Product.Services.ProductServices;

public interface IDimensionsFieldMaskService
{
    public Dimensions GetUpdatedDimensionValues(
        UpdateProductRequest request,
        Dimensions currentDimensions);
}
