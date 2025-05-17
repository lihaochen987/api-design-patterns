// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.User.Controllers;

namespace backend.User.ApplicationLayer.Queries.ListUsers;

public record ListUsersQuery : IQuery<PagedUsers>
{
    public required ListUsersRequest Request { get; init; }
}
