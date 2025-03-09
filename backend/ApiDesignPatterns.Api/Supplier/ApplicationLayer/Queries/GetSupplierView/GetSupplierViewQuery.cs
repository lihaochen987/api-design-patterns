// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.Supplier.DomainModels;

namespace backend.Supplier.ApplicationLayer.Queries.GetSupplierView;

public record GetSupplierViewQuery : IQuery<SupplierView?>
{
    public long Id { get; init; }
}
