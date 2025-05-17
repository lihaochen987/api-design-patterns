// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.User.DomainModels;
using backend.User.InfrastructureLayer.Database.UserView;

namespace backend.User.ApplicationLayer.Queries.GetUserView;

public class GetUserViewHandler(
    IUserViewRepository repository)
    : IAsyncQueryHandler<GetUserViewQuery, UserView?>
{
    public async Task<UserView?> Handle(GetUserViewQuery query)
    {
        UserView? user = await repository.GetUserView(query.Id);
        return user;
    }
}
