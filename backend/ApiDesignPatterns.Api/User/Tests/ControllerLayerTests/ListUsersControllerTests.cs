// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.User.ApplicationLayer.Queries.ListUsers;
using backend.User.Controllers;
using backend.User.DomainModels;
using backend.User.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace backend.User.Tests.ControllerLayerTests;

public class ListUsersControllerTests : ListUsersControllerTestBase
{
    [Fact]
    public async Task ListUsers_ShouldReturnAllUsers_WhenNoPageTokenProvided()
    {
        List<UserView> userViews = new UserViewTestDataBuilder().CreateMany(4).ToList();
        ListUsersRequest request = new() { MaxPageSize = 4 };
        Mock
            .Get(MockListUsers)
            .Setup(svc => svc.Handle(It.Is<ListUsersQuery>(q =>
                q.Request.Filter == request.Filter &&
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedUsers(userViews, null));
        ListUsersController sut = ListUsersController();

        var result = await sut.ListUsers(request);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = result.Result.As<OkObjectResult>();
        response.Should().NotBeNull();
        var listUsersResponse = response.Value.Should().BeOfType<ListUsersResponse>().Subject;
        listUsersResponse.Results.Should().HaveCount(4);
        listUsersResponse.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task ListUsers_ShouldReturnUsersAfterPageToken_WhenPageTokenProvided()
    {
        List<UserView> userViewList = new UserViewTestDataBuilder().CreateMany(4).ToList();
        var expectedPageResults = userViewList.Skip(2).Take(2).ToList();
        ListUsersRequest request = new() { PageToken = "2", MaxPageSize = 2 };
        Mock
            .Get(MockListUsers)
            .Setup(svc => svc.Handle(It.Is<ListUsersQuery>(q =>
                q.Request.Filter == request.Filter &&
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedUsers(expectedPageResults, null));
        ListUsersController sut = ListUsersController();

        var result = await sut.ListUsers(request);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = result.Result.As<OkObjectResult>();
        response.Should().NotBeNull();
        var listUsersResponse = response.Value.Should().BeOfType<ListUsersResponse>().Subject;
        listUsersResponse.Results.Should().HaveCount(2);
        listUsersResponse.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task ListUsers_ShouldReturnNextPageToken_WhenMoreUsersExist()
    {
        List<UserView> users = new UserViewTestDataBuilder().CreateMany(20).ToList();
        List<UserView> firstPageUsers = users.Take(2).ToList();
        ListUsersRequest request = new() { MaxPageSize = 2 };
        Mock
            .Get(MockListUsers)
            .Setup(svc => svc.Handle(It.Is<ListUsersQuery>(q =>
                q.Request.Filter == request.Filter &&
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedUsers(firstPageUsers, "2"));
        ListUsersController sut = ListUsersController();

        var result = await sut.ListUsers(request);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = result.Result.As<OkObjectResult>();
        response.Should().NotBeNull();
        var listUsersResponse = response.Value.Should().BeOfType<ListUsersResponse>().Subject;
        listUsersResponse.Results.Should().HaveCount(2);
        listUsersResponse.NextPageToken.Should().BeEquivalentTo("2");
    }

    [Fact]
    public async Task ListUsers_ShouldUseDefaults_WhenPageTokenAndMaxPageSizeNotProvided()
    {
        List<UserView> users = new UserViewTestDataBuilder().CreateMany(20).ToList();
        List<UserView> defaultPageUsers = users.Take(DefaultMaxPageSize).ToList();
        ListUsersRequest request = new();
        Mock
            .Get(MockListUsers)
            .Setup(svc => svc.Handle(It.Is<ListUsersQuery>(q =>
                q.Request.Filter == request.Filter &&
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedUsers(defaultPageUsers, DefaultMaxPageSize.ToString()));
        ListUsersController sut = ListUsersController();

        var result = await sut.ListUsers(request);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = result.Result.As<OkObjectResult>();
        response.Should().NotBeNull();
        var listUsersResponse = response.Value.Should().BeOfType<ListUsersResponse>().Subject;
        listUsersResponse.Results.Should().HaveCount(DefaultMaxPageSize);
        listUsersResponse.NextPageToken.Should().BeEquivalentTo(DefaultMaxPageSize.ToString());
    }

    [Fact]
    public async Task ListUsers_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        ListUsersRequest request = new() { MaxPageSize = 2 };
        Mock
            .Get(MockListUsers)
            .Setup(svc => svc.Handle(It.Is<ListUsersQuery>(q =>
                q.Request.Filter == request.Filter &&
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedUsers([], null));
        ListUsersController sut = ListUsersController();

        var result = await sut.ListUsers(request);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = result.Result.As<OkObjectResult>();
        response.Should().NotBeNull();
        var listUsersResponse = response.Value.Should().BeOfType<ListUsersResponse>().Subject;
        listUsersResponse.Results.Should().BeEmpty();
        listUsersResponse.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task ListUsers_WithFilterAndPagination_ReturnsCorrectResults()
    {
        var user = new UserViewTestDataBuilder()
            .WithId(2)
            .WithFullName("John Doe")
            .Build();
        var filteredUsers = new List<UserView> { user };
        ListUsersRequest request = new() { Filter = "FullName == 'John Doe'", MaxPageSize = 2, PageToken = "1" };
        Mock
            .Get(MockListUsers)
            .Setup(svc => svc.Handle(It.Is<ListUsersQuery>(q =>
                q.Request.Filter == request.Filter &&
                q.Request.MaxPageSize == request.MaxPageSize &&
                q.Request.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedUsers(filteredUsers, "2"));
        ListUsersController sut = ListUsersController();

        var result = await sut.ListUsers(request);

        result.Result.Should().BeOfType<OkObjectResult>();
        var response = result.Result.As<OkObjectResult>();
        var listUsersResponse = response.Value.Should().BeOfType<ListUsersResponse>().Subject;
        listUsersResponse.Results.Should().HaveCount(1);
        listUsersResponse.NextPageToken.Should().Be("2");
    }
}
