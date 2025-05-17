// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.User.ApplicationLayer.Commands.DeleteUser;
using backend.User.ApplicationLayer.Queries.GetUser;
using backend.User.Controllers;
using backend.User.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace backend.User.Tests.ControllerLayerTests;

public class DeleteUserControllerTests : DeleteUserControllerTestBase
{
    [Fact]
    public async Task DeleteUser_UserExists_ReturnsNoContent()
    {
        DomainModels.User user = new UserTestDataBuilder().Build();
        Mock
            .Get(MockGetUserHandler)
            .Setup(svc => svc.Handle(It.Is<GetUserQuery>(q => q.Id == user.Id)))
            .ReturnsAsync(user);
        DeleteUserController sut = DeleteUserController();

        ActionResult result = await sut.DeleteUser(user.Id, new DeleteUserRequest());

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteUser_UserDoesNotExist_ReturnsNotFound()
    {
        DomainModels.User user = new UserTestDataBuilder().Build();
        Mock
            .Get(MockGetUserHandler)
            .Setup(svc => svc.Handle(It.Is<GetUserQuery>(q => q.Id == user.Id)))
            .ReturnsAsync((DomainModels.User?)null);
        DeleteUserController sut = DeleteUserController();

        ActionResult result = await sut.DeleteUser(user.Id, new DeleteUserRequest());

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteUser_HandlesDeleteFailure_ThrowsException()
    {
        DomainModels.User user = new UserTestDataBuilder().Build();
        Mock
            .Get(MockGetUserHandler)
            .Setup(svc => svc.Handle(It.Is<GetUserQuery>(q => q.Id == user.Id)))
            .ReturnsAsync(user);
        Mock
            .Get(MockDeleteUserHandler)
            .Setup(svc => svc.Handle(It.IsAny<DeleteUserCommand>()))
            .ThrowsAsync(new Exception("Failed to delete user"));
        DeleteUserController sut = DeleteUserController();

        Func<Task> act = async () => await sut.DeleteUser(user.Id, new DeleteUserRequest());

        await act.Should().ThrowAsync<Exception>();
    }
}
