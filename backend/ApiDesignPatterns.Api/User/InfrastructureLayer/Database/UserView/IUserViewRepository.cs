// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.User.InfrastructureLayer.Database.UserView;

public interface IUserViewRepository
{
    Task<DomainModels.UserView?> GetUserView(long id);

    Task<(List<DomainModels.UserView>, string?)> ListUsersAsync(
        string? pageToken,
        string? filter,
        int maxPageSize);
}
