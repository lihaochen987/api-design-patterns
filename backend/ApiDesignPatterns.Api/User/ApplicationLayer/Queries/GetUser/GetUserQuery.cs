// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;

namespace backend.User.ApplicationLayer.Queries.GetUser;

public record GetUserQuery : IQuery<DomainModels.User?>
{
    public long Id { get; init; }
}
