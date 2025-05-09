// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;

namespace backend.Address.ApplicationLayer.Queries.GetAddress;

public class GetAddressQuery : IQuery<DomainModels.Address?>
{
    public long Id { get; init; }
}
