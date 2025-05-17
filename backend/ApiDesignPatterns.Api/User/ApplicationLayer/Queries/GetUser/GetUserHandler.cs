// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.User.InfrastructureLayer.Database.User;

namespace backend.User.ApplicationLayer.Queries.GetUser;

public class GetUserHandler(IUserRepository repository) : IAsyncQueryHandler<GetUserQuery, DomainModels.User?>
{
    public async Task<DomainModels.User?> Handle(GetUserQuery query)
    {
        DomainModels.User? user = await repository.GetUserAsync(query.Id);
        return user;
    }
}
