// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using backend.Shared;
using backend.User.DomainModels;
using backend.User.InfrastructureLayer.Database.UserView;
using backend.User.Tests.TestHelpers.Builders;

namespace backend.User.Tests.TestHelpers.Fakes;

public class UserViewRepositoryFake(
    PaginateService<UserView> paginateService)
    : Collection<UserView>, IUserViewRepository
{
    public void AddUserView(string firstName, string lastName, string email)
    {
        var userView = new UserViewTestDataBuilder()
            .WithFullName(firstName + " " + lastName)
            .WithEmail(email)
            .Build();
        Add(userView);
    }

    public Task<UserView?> GetUserView(long id)
    {
        UserView? userView = this.FirstOrDefault(s => s.Id == id);
        return Task.FromResult(userView);
    }

    public Task<(List<UserView>, string?)> ListUsersAsync(
        string? pageToken,
        string? filter,
        int maxPageSize)
    {
        var query = this.AsEnumerable();

        if (!string.IsNullOrEmpty(pageToken) && long.TryParse(pageToken, out long lastSeenUser))
        {
            query = query.Where(s => s.Id > lastSeenUser);
        }

        if (!string.IsNullOrEmpty(filter))
        {
            if (filter.Contains("Email.endsWith"))
            {
                string value = filter.Split('(', ')')[1];
                query = query.Where(s => s.Email.EndsWith(value));
            }
            else if (filter.Contains("FullName =="))
            {
                string value = filter.Split('"')[1];
                query = query.Where(s => s.FullName == value);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        var users = query
            .OrderBy(s => s.Id)
            .Take(maxPageSize + 1)
            .ToList();

        List<UserView> paginatedUsers =
            paginateService.Paginate(users, maxPageSize, out string? nextPageToken);

        return Task.FromResult((paginatedUsers, nextPageToken));
    }
}
