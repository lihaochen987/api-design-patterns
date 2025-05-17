// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.User.ApplicationLayer.Queries.GetUserView;
using backend.User.DomainModels;
using backend.User.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.User.Tests.ApplicationLayerTests;

public class GetUserViewHandlerTests : GetUserViewHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsNull_WhenUserDoesNotExist()
    {
        UserView expectedUser = new UserViewTestDataBuilder().Build();
        var sut = GetUserViewHandler();

        UserView? result = await sut.Handle(new GetUserViewQuery { Id = expectedUser.Id });

        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ReturnsUser_WhenUserExists()
    {
        UserView expectedUser = new UserViewTestDataBuilder().Build();
        Repository.Add(expectedUser);
        var sut = GetUserViewHandler();

        UserView? result = await sut.Handle(new GetUserViewQuery { Id = expectedUser.Id });

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedUser);
    }
}
