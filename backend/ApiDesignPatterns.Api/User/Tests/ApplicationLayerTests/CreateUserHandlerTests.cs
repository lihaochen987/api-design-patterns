// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.User.ApplicationLayer.Commands.CreateUser;
using backend.User.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.User.Tests.ApplicationLayerTests;

public class CreateUserHandlerTests : CreateUserHandlerTestBase
{
    [Fact]
    public async Task Handle_CallsRepositoryWithCorrectUser()
    {
        var userToCreate = new UserTestDataBuilder().Build();
        var command = new CreateUserCommand { User = userToCreate };
        ICommandHandler<CreateUserCommand> sut = CreateUserService();

        await sut.Handle(command);

        Repository.IsDirty.Should().BeTrue();
        var createdUser = Repository.First();
        createdUser.Id.Should().NotBe(0);
        createdUser.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        createdUser.Should().BeEquivalentTo(userToCreate,
            options => options.Excluding(s => s.CreatedAt));
    }

    [Fact]
    public async Task Handle_PersistsWhenCalledTwice()
    {
        var firstUserToCreate = new UserTestDataBuilder().Build();
        var secondUserToCreate = new UserTestDataBuilder().Build();
        var firstCommand = new CreateUserCommand { User = firstUserToCreate };
        var secondCommand = new CreateUserCommand { User = secondUserToCreate };
        ICommandHandler<CreateUserCommand> sut = CreateUserService();

        await sut.Handle(firstCommand);
        await sut.Handle(secondCommand);

        Repository.IsDirty.Should().BeTrue();

        var firstUser = Repository.First(x => x.Id == firstUserToCreate.Id);
        firstUser.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        firstUser.Should().BeEquivalentTo(firstUserToCreate,
            options => options.Excluding(s => s.CreatedAt));

        var secondUser = Repository.First(x => x.Id == secondUserToCreate.Id);
        secondUser.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        secondUser.Should().BeEquivalentTo(secondUserToCreate,
            options => options.Excluding(s => s.CreatedAt));
    }
}
