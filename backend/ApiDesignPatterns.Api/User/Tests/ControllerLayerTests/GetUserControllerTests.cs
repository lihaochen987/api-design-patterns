// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Net;
using AutoFixture;
using backend.Product.Controllers.Product;
using backend.User.ApplicationLayer.Queries.GetUserView;
using backend.User.Controllers;
using backend.User.DomainModels;
using backend.User.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace backend.User.Tests.ControllerLayerTests;

public class GetUserControllerTests : GetUserControllerTestBase
{
    [Fact]
    public async Task GetUser_ReturnsOkResult_WhenUserExists()
    {
        long userId = Fixture.Create<long>();
        var userView = new UserViewTestDataBuilder()
            .WithId(userId)
            .WithFullName("John Doe")
            .WithEmail("john.doe@example.com")
            .Build();
        var request = Fixture.Build<GetUserRequest>()
            .With(r => r.FieldMask, ["FullName", "Email"])
            .Create();
        Mock
            .Get(MockGetUserView)
            .Setup(service => service.Handle(It.Is<GetUserViewQuery>(q => q.Id == userId)))
            .ReturnsAsync(userView);
        GetUserController sut = GetUserController();

        ActionResult<GetProductResponse> result = await sut.GetUser(userId, request);

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().NotBeNull();
        string? jsonResult = ((string)okResult.Value);
        jsonResult.Should().Contain("John Doe");
        jsonResult.Should().Contain("john.doe@example.com");
    }

    [Fact]
    public async Task GetUser_ReturnsNotFound_WhenUserDoesNotExist()
    {
        long userId = Fixture.Create<long>();
        var request = Fixture.Create<GetUserRequest>();
        Mock
            .Get(MockGetUserView)
            .Setup(service => service.Handle(It.Is<GetUserViewQuery>(q => q.Id == userId)))
            .ReturnsAsync((UserView?)null);
        GetUserController sut = GetUserController();

        ActionResult<GetProductResponse> result = await sut.GetUser(userId, request);

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetUser_SerializesWithFieldMaskCorrectly()
    {
        long userId = Fixture.Create<long>();
        var userView = new UserViewTestDataBuilder()
            .WithId(userId)
            .WithFullName("John Doe")
            .WithEmail("john.doe@example.com")
            .Build();
        var request = Fixture.Build<GetUserRequest>()
            .With(r => r.FieldMask, ["FullName"])
            .Create();
        Mock
            .Get(MockGetUserView)
            .Setup(service => service.Handle(It.Is<GetUserViewQuery>(q => q.Id == userId)))
            .ReturnsAsync(userView);
        GetUserController sut = GetUserController();

        ActionResult<GetProductResponse> result = await sut.GetUser(userId, request);

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().NotBeNull();
        string? jsonResult = (string)okResult.Value;
        jsonResult.Should().Contain("John Doe");
        jsonResult.Should().NotContain("john.doe@example.com");
    }
}
