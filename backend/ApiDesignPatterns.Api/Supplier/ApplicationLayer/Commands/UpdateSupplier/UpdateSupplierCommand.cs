// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.Controllers;

namespace backend.Supplier.ApplicationLayer.Commands.UpdateSupplier;

public record UpdateSupplierCommand
{
    public required UpdateSupplierRequest Request { get; init; }
    public required DomainModels.Supplier Supplier { get; init; }
}
