// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.User.DomainModels;

namespace backend.Inventory.ApplicationLayer.Queries.GetUsersByIds;

public class GetUsersByIdsQuery : IQuery<List<UserView>>
{
    public required List<long> UserIds { get; set; }
}
