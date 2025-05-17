// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.User.ApplicationLayer.Queries.GetUser;
using backend.User.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.User.Tests.ApplicationLayerTests;

public class GetUserHandlerTests : GetUserHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsUser_WhenUserExists()
    {
        DomainModels.User expectedUser = new UserTestDataBuilder().Build();
        Repository.Add(expectedUser);
        var sut = GetUserHandler();

        DomainModels.User? result = await sut.Handle(new GetUserQuery { Id = expectedUser.Id });

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedUser);
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenUserDoesNotExist()
    {
        DomainModels.User nonExistentUser = new UserTestDataBuilder().Build();
        var sut = GetUserHandler();

        DomainModels.User? result = await sut.Handle(new GetUserQuery { Id = nonExistentUser.Id });

        result.Should().BeNull();
    }
}
