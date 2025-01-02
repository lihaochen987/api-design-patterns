// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Services.ProductServices;
using backend.Supplier.Services;

namespace backend.Shared.FieldPath;

public class FieldPathAdapter : IFieldPathAdapter
{
    public IFieldPaths GetFieldPaths(string context)
    {
        return context switch
        {
            "Supplier" => new SupplierFieldPaths(),
            "Product" => new ProductFieldPaths(),
            _ => throw new ArgumentException($"Invalid context: {context}", nameof(context))
        };
    }
}
