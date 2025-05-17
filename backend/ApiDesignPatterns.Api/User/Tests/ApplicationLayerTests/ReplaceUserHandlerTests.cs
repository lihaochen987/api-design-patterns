// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.User.ApplicationLayer.Commands.ReplaceUser;
using backend.User.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.User.Tests.ApplicationLayerTests;

public class ReplaceUserHandlerTests : ReplaceUserHandlerTestBase
{
    [Fact]
    public async Task Handle_ReplaceUser_UpdatesAllUserInformation()
    {
        var existingUser = new UserTestDataBuilder().Build();
        var replacedUser = new UserTestDataBuilder().WithId(existingUser.Id).Build();
        Repository.Add(existingUser);
        var command = new ReplaceUserCommand { User = replacedUser, UserId = existingUser.Id };
        ICommandHandler<ReplaceUserCommand> sut = ReplaceUserHandler();
        Repository.IsDirty = false;

        await sut.Handle(command);

        Repository.IsDirty.Should().BeTrue();
        Repository.First().Should().BeEquivalentTo(replacedUser,
            options => options.Excluding(s => s.CreatedAt));
        Repository.First().CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}
