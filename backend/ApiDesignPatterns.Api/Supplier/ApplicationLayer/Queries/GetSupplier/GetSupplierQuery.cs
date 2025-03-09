// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;

namespace backend.Supplier.ApplicationLayer.Queries.GetSupplier;

public record GetSupplierQuery : IQuery<DomainModels.Supplier?>
{
    public long Id { get; init; }
}
