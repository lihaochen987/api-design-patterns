// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Supplier.DomainModels.ValueObjects;

namespace backend.Supplier.InfrastructureLayer.Database.Mapping;

public record AddressWithSupplierId : Address
{
    public long SupplierId { get; init; }
}
