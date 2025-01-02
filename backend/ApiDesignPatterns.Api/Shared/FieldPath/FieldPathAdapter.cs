// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Services.ProductServices;
using backend.Supplier.Services;

namespace backend.Shared.FieldPath;

public class FieldPathAdapter : IFieldPathAdapter
{
    public HashSet<string> GetFieldPaths(string context)
    {
        return context switch
        {
            "Supplier" => new SupplierFieldPaths().ValidPaths,
            "Product" => new ProductFieldPaths().ValidPaths,
            _ => throw new ArgumentException($"Invalid context: {context}", nameof(context))
        };
    }
}
