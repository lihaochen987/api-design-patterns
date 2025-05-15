// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Address.ApplicationLayer.Commands.DeleteAddress;
using backend.Address.Tests.TestHelpers.Builders;
using backend.Shared.CommandHandler;
using FluentAssertions;
using Xunit;

namespace backend.Address.Tests.ApplicationLayerTests;

public class DeleteAddressHandlerTests : DeleteAddressHandlerTestBase
{
    [Fact]
    public async Task Handle_CallsRepositoryWithCorrectAddress()
    {
        DomainModels.Address addressToDelete = new AddressTestDataBuilder().Build();
        Repository.Add(addressToDelete);
        ICommandHandler<DeleteAddressCommand> sut = DeleteAddressService();

        await sut.Handle(new DeleteAddressCommand { Id = addressToDelete.Id });

        Repository.IsDirty.Should().BeTrue();
        Repository.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_DoesNotThrowException_WhenAddressDoesNotExist()
    {
        long nonExistentId = Fixture.Create<long>();
        ICommandHandler<DeleteAddressCommand> sut = DeleteAddressService();

        await sut.Handle(new DeleteAddressCommand { Id = nonExistentId });

        Repository.IsDirty.Should().BeFalse();
        Repository.Should().BeEmpty();
    }
}
