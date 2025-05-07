// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.Supplier.DomainModels;
using backend.Supplier.InfrastructureLayer.Database.SupplierView;

namespace backend.Inventory.ApplicationLayer.Queries.GetSuppliersByIds;

public class GetSuppliersByIdsHandler(ISupplierViewRepository repository)
    : IAsyncQueryHandler<GetSuppliersByIdsQuery, List<SupplierView>>
{
    public async Task<List<SupplierView>> Handle(GetSuppliersByIdsQuery query)
    {
        var tasks = query.SupplierIds.Select(repository.GetSupplierView).ToList();

        var suppliers = (await Task.WhenAll(tasks)).Where(x => x != null).ToList();

        return suppliers!;
    }
}
