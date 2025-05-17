// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Queries.GetUsersByIds;
using backend.User.DomainModels;
using backend.User.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public class GetUsersByIdsHandlerTests : GetUsersByIdsHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsUsers_WhenUsersExist()
    {
        var userOne = new UserViewTestDataBuilder().Build();
        var userTwo = new UserViewTestDataBuilder().Build();
        var expectedUsers = new List<UserView> { userOne, userTwo };
        var userIds = expectedUsers.Select(s => s.Id).ToList();
        Repository.Add(userOne);
        Repository.Add(userTwo);
        var sut = GetUsersByIdsHandler();
        var query = new GetUsersByIdsQuery { UserIds = userIds };

        var result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedUsers);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoUsersExist()
    {
        var userIds = new List<long> { Fixture.Create<long>(), Fixture.Create<long>() };
        var sut = GetUsersByIdsHandler();
        var query = new GetUsersByIdsQuery { UserIds = userIds };

        var result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ReturnsOnlyFoundUsers_WhenSomeExist()
    {
        var existingUser = new UserViewTestDataBuilder().Build();
        var existingUsers = new List<UserView> { existingUser };
        var userIds = new List<long> { existingUser.Id, Fixture.Create<long>() };
        Repository.Add(existingUser);
        var sut = GetUsersByIdsHandler();
        var query = new GetUsersByIdsQuery { UserIds = userIds };

        var result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.Should().BeEquivalentTo(existingUsers);
    }
}
