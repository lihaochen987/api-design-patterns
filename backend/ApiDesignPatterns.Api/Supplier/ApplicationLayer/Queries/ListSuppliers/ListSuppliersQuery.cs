// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.Supplier.Controllers;

namespace backend.Supplier.ApplicationLayer.Queries.ListSuppliers;

public record ListSuppliersQuery : IQuery<PagedSuppliers>
{
    public required ListSuppliersRequest Request { get; init; }
}
