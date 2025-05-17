// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using backend.User.InfrastructureLayer.Database.User;

namespace backend.User.Tests.TestHelpers.Fakes;

public class UserRepositoryFake : Collection<DomainModels.User>, IUserRepository
{
    public bool IsDirty { get; set; }

    public Task<DomainModels.User?> GetUserAsync(long id)
    {
        DomainModels.User? user = this.FirstOrDefault(r => r.Id == id);
        return Task.FromResult(user);
    }

    public Task DeleteUserAsync(long id)
    {
        var user = this.FirstOrDefault(r => r.Id == id);
        if (user == null)
        {
            return Task.CompletedTask;
        }

        Remove(user);
        IsDirty = true;
        return Task.CompletedTask;
    }

    public Task<long> CreateUserAsync(DomainModels.User user)
    {
        IsDirty = true;
        Add(user);
        return Task.FromResult(user.Id);
    }

    public Task<long> UpdateUserAsync(DomainModels.User newUser, DomainModels.User oldUser)
    {
        int index = IndexOf(this.FirstOrDefault(r => r.Id == newUser.Id) ??
                            throw new InvalidOperationException());
        this[index] = newUser;
        IsDirty = true;

        return Task.FromResult(newUser.Id);
    }

    public Task<long> ReplaceUserAsync(DomainModels.User user)
    {
        int index = IndexOf(this.FirstOrDefault(r => r.Id == user.Id) ??
                            throw new InvalidOperationException());
        this[index] = user;
        IsDirty = true;

        return Task.FromResult(user.Id);
    }
}
