// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.User.ApplicationLayer.Commands.UpdateUser;
using backend.User.Controllers;
using backend.User.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.User.Tests.ApplicationLayerTests;

public class UpdateUserHandlerTests : UpdateUserHandlerTestBase
{
    [Fact]
    public async Task UpdateUserAsync_WithMultipleFieldsInFieldMask_ShouldUpdateOnlySpecifiedFields()
    {
        var user = new UserTestDataBuilder()
            .WithId(3)
            .Build();
        Repository.Add(user);
        Repository.IsDirty = false;
        var request = new UpdateUserRequest
        {
            FirstName = "Updated First",
            LastName = "Updated Last",
            Email = "new@email.com",
            FieldMask = ["firstname", "email", "lastname"]
        };
        ICommandHandler<UpdateUserCommand> sut = UpdateUserHandler();

        await sut.Handle(new UpdateUserCommand { User = user, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().FirstName.Value.Should().BeEquivalentTo(request.FirstName);
        Repository.First().LastName.Value.Should().BeEquivalentTo(request.LastName);
        Repository.First().Email.Value.Should().BeEquivalentTo(request.Email);
    }

    [Fact]
    public async Task UpdateUserAsync_WithReferenceFieldMasks_ShouldUpdateOnlySpecifiedFields()
    {
        var user = new UserTestDataBuilder()
            .WithId(3)
            .Build();
        Repository.Add(user);
        Repository.IsDirty = false;
        var request = new UpdateUserRequest
        {
            AddressIds = [1, 2, 3],
            PhoneNumberIds = [4, 5, 6],
            FirstName = "Updated First",
            LastName = "Updated Last",
            Email = "new@email.com",
            FieldMask = ["addressids", "phonenumberids"]
        };
        ICommandHandler<UpdateUserCommand> sut = UpdateUserHandler();

        await sut.Handle(new UpdateUserCommand { User = user, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().AddressIds.Should().BeEquivalentTo(request.AddressIds);
        Repository.First().PhoneNumberIds.Should().BeEquivalentTo(request.PhoneNumberIds);
        Repository.First().FirstName.Should().BeEquivalentTo(user.FirstName);
        Repository.First().LastName.Should().BeEquivalentTo(user.LastName);
        Repository.First().Email.Should().BeEquivalentTo(user.Email);
    }
}
