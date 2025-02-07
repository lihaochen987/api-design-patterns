// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.ApplicationLayer.Commands.CreateSupplier;

public record CreateSupplierCommand
{
    public required DomainModels.Supplier Supplier { get; init; }
}
