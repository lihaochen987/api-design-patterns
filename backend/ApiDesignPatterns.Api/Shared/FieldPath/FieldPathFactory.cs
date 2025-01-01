// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.Services.ProductServices;
using backend.Supplier.Services;

namespace backend.Shared.FieldPath;

public class FieldPathFactory : IFieldPathFactory
{
    public IFieldPaths Create(string context)
    {
        if (context == "Supplier")
        {
            return new SupplierFieldPaths();
        }

        return new ProductFieldPaths();
    }
}
