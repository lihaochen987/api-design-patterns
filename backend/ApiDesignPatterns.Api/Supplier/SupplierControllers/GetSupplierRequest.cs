// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.SupplierControllers;

public record GetSupplierRequest
{
    public List<string> FieldMask { get; set; } = ["*"];
}
