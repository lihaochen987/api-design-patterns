// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.DomainModels.Views;
using backend.Shared.QueryHandler;

namespace backend.Inventory.ApplicationLayer.Queries.GetProductsByIds;

public class GetProductsByIdsQuery : IQuery<List<ProductView>>
{
    public required List<long> ProductIds { get; set; }
}
