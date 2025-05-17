// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.User.DomainModels;
using backend.User.InfrastructureLayer.Database.UserView;

namespace backend.User.ApplicationLayer.Queries.ListUsers;

public class ListUsersHandler(IUserViewRepository repository)
    : IAsyncQueryHandler<ListUsersQuery, PagedUsers>
{
    public async Task<PagedUsers> Handle(ListUsersQuery query)
    {
        (List<UserView> users, string? nextPageToken) = await repository.ListUsersAsync(
            query.Request.PageToken,
            query.Request.Filter,
            query.Request.MaxPageSize);
        return new PagedUsers(users, nextPageToken);
    }
}
