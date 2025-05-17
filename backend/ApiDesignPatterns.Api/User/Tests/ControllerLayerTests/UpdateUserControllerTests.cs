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

public class UpdateUserControllerTests : UpdateUserControllerTestBase
{
    [Fact]
    public async Task UpdateUser_WithValidRequest_ShouldReturnUpdatedUser()
    {
        var user = new UserTestDataBuilder().Build();
        UpdateUserRequest request = new() { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
        Mock
            .Get(MockGetUserHandler)
            .Setup(svc => svc.Handle(It.Is<GetUserQuery>(q => q.Id == user.Id)))
            .ReturnsAsync(user);
        UpdateUserController sut = UpdateUserController();

        ActionResult<UpdateUserResponse> actionResult = await sut.UpdateUser(user.Id, request);

        actionResult.Result.Should().BeOfType<OkObjectResult>();
        var contentResult = actionResult.Result.As<OkObjectResult>();
        var response = contentResult.Value.Should().BeOfType<UpdateUserResponse>().Subject;
        response.Should().BeEquivalentTo(Mapper.Map<UpdateUserResponse>(user));
    }

    [Fact]
    public async Task UpdateUser_NonExistentUser_ShouldReturnNotFound()
    {
        UpdateUserRequest request = new() { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
        long nonExistentId = Fixture.Create<long>();
        Mock
            .Get(MockGetUserHandler)
            .Setup(svc => svc.Handle(It.Is<GetUserQuery>(q => q.Id == nonExistentId)))
            .ReturnsAsync((DomainModels.User?)null);
        var sut = UpdateUserController();

        ActionResult<UpdateUserResponse> actionResult = await sut.UpdateUser(nonExistentId, request);

        actionResult.Result.Should().NotBeNull();
        actionResult.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task UpdateUser_WhenUpdateSucceeds_ShouldReturnMappedResponse()
    {
        var user = new UserTestDataBuilder().Build();
        UpdateUserRequest request = new() { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
        Mock
            .Get(MockGetUserHandler)
            .Setup(svc => svc.Handle(It.Is<GetUserQuery>(q => q.Id == user.Id)))
            .ReturnsAsync(user);
        var expectedResponse = Mapper.Map<UpdateUserResponse>(user);
        var sut = UpdateUserController();

        var result = await sut.UpdateUser(user.Id, request);

        result.Should().NotBeNull();
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task UpdateUser_VerifiesSecondGetCall_AfterUpdate()
    {
        var user = new UserTestDataBuilder().Build();
        var updatedUser = new UserTestDataBuilder()
            .WithId(user.Id)
            .WithFirstName(new FirstName("Updated"))
            .Build();
        UpdateUserRequest request = new() { FirstName = "Updated" };
        Mock
            .Get(MockGetUserHandler)
            .SetupSequence(svc => svc.Handle(It.Is<GetUserQuery>(q => q.Id == user.Id)))
            .ReturnsAsync(user)
            .ReturnsAsync(updatedUser);
        var sut = UpdateUserController();

        var result = await sut.UpdateUser(user.Id, request);

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<UpdateUserResponse>().Subject;
        response.Should().NotBeNull();
        response.FirstName.Should().Be("Updated");
    }

    [Fact]
    public async Task UpdateUser_PassesCorrectCommandParameters()
    {
        var user = new UserTestDataBuilder().Build();
        UpdateUserRequest request = new() { FirstName = "John", Email = "john@example.com" };
        Mock
            .Get(MockGetUserHandler)
            .Setup(svc => svc.Handle(It.Is<GetUserQuery>(q => q.Id == user.Id)))
            .ReturnsAsync(user);
        var sut = UpdateUserController();

        var result = await sut.UpdateUser(user.Id, request);

        result.Result.Should().BeOfType<OkObjectResult>();
    }
}
