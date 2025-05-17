// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Shared.CommandHandler;
using backend.User.ApplicationLayer.Commands.DeleteUser;
using backend.User.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.User.Tests.ApplicationLayerTests;

public class DeleteUserHandlerTests : DeleteUserHandlerTestBase
{
    [Fact]
    public async Task Handle_CallsRepositoryWithCorrectUser()
    {
        DomainModels.User userToDelete = new UserTestDataBuilder().Build();
        Repository.Add(userToDelete);
        ICommandHandler<DeleteUserCommand> sut = DeleteUserService();

        await sut.Handle(new DeleteUserCommand { Id = userToDelete.Id });

        Repository.IsDirty.Should().BeTrue();
        Repository.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_DoesNotThrowException_WhenUserDoesNotExist()
    {
        long nonExistentId = Fixture.Create<long>();
        ICommandHandler<DeleteUserCommand> sut = DeleteUserService();

        await sut.Handle(new DeleteUserCommand { Id = nonExistentId });

        Repository.IsDirty.Should().BeFalse();
    }
}
