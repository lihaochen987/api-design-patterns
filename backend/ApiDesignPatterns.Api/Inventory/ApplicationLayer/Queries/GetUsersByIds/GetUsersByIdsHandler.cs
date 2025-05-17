// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.QueryHandler;
using backend.User.DomainModels;
using backend.User.InfrastructureLayer.Database.UserView;

namespace backend.Inventory.ApplicationLayer.Queries.GetUsersByIds;

public class GetUsersByIdsHandler(IUserViewRepository repository)
    : IAsyncQueryHandler<GetUsersByIdsQuery, List<UserView>>
{
    public async Task<List<UserView>> Handle(GetUsersByIdsQuery query)
    {
        var tasks = query.UserIds.Select(repository.GetUserView).ToList();

        var users = (await Task.WhenAll(tasks)).Where(x => x != null).ToList();

        return users!;
    }
}
