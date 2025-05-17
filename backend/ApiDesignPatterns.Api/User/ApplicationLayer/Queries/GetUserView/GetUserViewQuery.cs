// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.User.DomainModels;

namespace backend.User.ApplicationLayer.Queries.GetUserView;

public record GetUserViewQuery : IQuery<UserView?>
{
    public long Id { get; init; }
}
