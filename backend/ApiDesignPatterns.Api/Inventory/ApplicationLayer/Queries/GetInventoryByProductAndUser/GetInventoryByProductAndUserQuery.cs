// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;

namespace backend.Inventory.ApplicationLayer.Queries.GetInventoryByProductAndUser;

public record GetInventoryByProductAndUserQuery : IQuery<DomainModels.Inventory?>
{
    public long ProductId { get; set; }
    public long UserId { get; set; }
}
