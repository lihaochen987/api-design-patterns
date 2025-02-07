// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.ApplicationLayer.Commands.DeleteSupplier;

public record DeleteSupplierCommand
{
    public long Id { get; init; }
}
