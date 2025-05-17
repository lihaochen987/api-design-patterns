// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.User.InfrastructureLayer.Database.User;

public interface IUserRepository
{
    Task<DomainModels.User?> GetUserAsync(long id);
    Task DeleteUserAsync(long id);
    Task<long> CreateUserAsync(DomainModels.User user);
    Task<long> UpdateUserAsync(DomainModels.User newUser, DomainModels.User oldUser);
    Task<long> ReplaceUserAsync(DomainModels.User user);
}
