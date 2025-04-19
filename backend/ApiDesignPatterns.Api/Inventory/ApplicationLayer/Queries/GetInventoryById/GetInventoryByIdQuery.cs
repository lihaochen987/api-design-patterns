// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;

namespace backend.Inventory.ApplicationLayer.Queries.GetInventoryById;

public record GetInventoryByIdQuery : IQuery<DomainModels.Inventory?>
{
    public long Id { get; init; }
}
