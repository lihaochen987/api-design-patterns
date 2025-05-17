// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.User.ApplicationLayer.Queries.GetUser;
using backend.User.Controllers;
using backend.User.DomainModels.ValueObjects;
using backend.User.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace backend.User.Tests.ControllerLayerTests;

public class ReplaceUserControllerTests : ReplaceUserControllerTestBase
{
    [Fact]
    public async Task ReplaceUser_ReturnsOkResponse_WhenUserReplacedSuccessfully()
    {
        var user = new UserTestDataBuilder().Build();
        var request = Mapper.Map<ReplaceUserRequest>(user);
        Mock
            .Get(GetUser)
            .Setup(x => x.Handle(It.Is<GetUserQuery>(q => q.Id == user.Id)))
            .ReturnsAsync(user);
        ReplaceUserController sut = GetReplaceUserController();

        var result = await sut.ReplaceUser(user.Id, request);

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task ReplaceUser_ReturnsNotFound_WhenUserDoesNotExist()
    {
        long nonExistentId = Fixture.Create<long>();
        var user = new UserTestDataBuilder().Build();
        var request = Mapper.Map<ReplaceUserRequest>(user);
        Mock
            .Get(GetUser)
            .Setup(x => x.Handle(It.Is<GetUserQuery>(q => q.Id == nonExistentId)))
            .ReturnsAsync((DomainModels.User?)null);
        ReplaceUserController sut = GetReplaceUserController();

        var result = await sut.ReplaceUser(nonExistentId, request);

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task ReplaceUser_HandlesMappingAndCommand_WithCorrectData()
    {
        var user = new UserTestDataBuilder()
            .WithFirstName(new FirstName("John"))
            .WithLastName(new LastName("Doe"))
            .WithEmail(new Email("john.doe@example.com"))
            .Build();
        var request = Mapper.Map<ReplaceUserRequest>(user);
        Mock
            .Get(GetUser)
            .Setup(x => x.Handle(It.Is<GetUserQuery>(q => q.Id == user.Id)))
            .ReturnsAsync(user);
        ReplaceUserController sut = GetReplaceUserController();

        var result = await sut.ReplaceUser(user.Id, request);

        result.Should().NotBeNull();
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<ReplaceUserResponse>().Subject;
        response.Should().NotBeNull();
        response.FirstName.Should().Be("John");
        response.LastName.Should().Be("Doe");
        response.Email.Should().Be("john.doe@example.com");
    }
}
