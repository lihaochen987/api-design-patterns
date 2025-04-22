// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.Supplier.DomainModels;

namespace backend.Inventory.ApplicationLayer.Queries.GetSuppliersByIds;

public class GetSuppliersByIdsQuery : IQuery<List<SupplierView>>
{
    public required List<long> SupplierIds { get; set; }
}
