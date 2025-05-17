// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.User.ApplicationLayer.Queries.ListUsers;
using backend.User.Controllers;
using FluentAssertions;
using Xunit;

namespace backend.User.Tests.ApplicationLayerTests;

public class ListUsersHandlerTests : ListUsersHandlerTestBase
{
    [Fact]
    public async Task Handle_ShouldReturnUsersAndNextPageToken()
    {
        var request = new ListUsersRequest { Filter = "Email.endsWith(@example.com)", MaxPageSize = 5 };
        Repository.AddUserView("John", "Doe", "john@example.com");
        Repository.AddUserView("Jane", "Smith", "jane@example.com");
        var sut = ListUsersViewHandler();

        PagedUsers result = await sut.Handle(new ListUsersQuery { Request = request });

        result.Users.Should().NotBeEmpty();
        result.Users.Should().HaveCount(2);
        result.NextPageToken.Should().BeNull();
        result.Users.Should().Contain(s => s.Email == "john@example.com");
        result.Users.Should().Contain(s => s.Email == "jane@example.com");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        var request = new ListUsersRequest();
        var sut = ListUsersViewHandler();

        PagedUsers result = await sut.Handle(new ListUsersQuery { Request = request });

        result.Users.Should().BeEmpty();
        result.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldFilterByName()
    {
        var request = new ListUsersRequest { Filter = "FullName == \"John Doe\"", MaxPageSize = 5 };
        Repository.AddUserView("John", "Doe", "john@example.com");
        Repository.AddUserView("Jane", "Smith", "jane@example.com");
        var sut = ListUsersViewHandler();

        PagedUsers result = await sut.Handle(new ListUsersQuery { Request = request });

        result.Users.Should().HaveCount(1);
        result.Users.Single().FullName.Should().Be("John Doe");
        result.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldRespectPageSize()
    {
        var request = new ListUsersRequest { MaxPageSize = 2 };
        Repository.AddUserView("John", "Doe", "john@example.com");
        Repository.AddUserView("Jane", "Smith", "jane@example.com");
        Repository.AddUserView("Bob", "Johnson", "bob@example.com");
        var sut = ListUsersViewHandler();

        PagedUsers result = await sut.Handle(new ListUsersQuery { Request = request });

        result.Users.Should().HaveCount(2);
        result.NextPageToken.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenRepositoryFails()
    {
        var request = new ListUsersRequest { Filter = "InvalidFilter == \"SomeValue\"", MaxPageSize = 5 };
        var sut = ListUsersViewHandler();

        Func<Task> act = async () => await sut.Handle(new ListUsersQuery { Request = request });

        await act.Should().ThrowAsync<ArgumentException>();
    }
}
