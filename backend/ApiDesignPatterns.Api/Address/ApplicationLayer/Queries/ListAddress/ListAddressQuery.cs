// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;

namespace backend.Address.ApplicationLayer.Queries.ListAddress;

public class ListAddressQuery : IQuery<PagedAddress>
{
    public string? Filter { get; init; }
    public string? PageToken { get; init; }
    public int MaxPageSize { get; init; }
}
