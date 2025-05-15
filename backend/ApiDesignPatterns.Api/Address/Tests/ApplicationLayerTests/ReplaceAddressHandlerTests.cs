// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Address.ApplicationLayer.Commands.ReplaceAddress;
using backend.Address.DomainModels.ValueObjects;
using backend.Address.Tests.TestHelpers.Builders;
using backend.Shared.CommandHandler;
using FluentAssertions;
using Xunit;

namespace backend.Address.Tests.ApplicationLayerTests;

public class ReplaceAddressHandlerTests : ReplaceAddressHandlerTestBase
{
    [Fact]
    public async Task Handle_UpdatesAddress_WhenValidAddressIsProvided()
    {
        var existingAddress = new AddressTestDataBuilder().Build();
        var replacementAddress = new AddressTestDataBuilder()
            .WithId(existingAddress.Id)
            .WithStreet(new Street("456 Updated Street"))
            .WithCity(new City("New City"))
            .Build();
        Repository.Add(existingAddress);
        var command = new ReplaceAddressCommand { Address = replacementAddress };
        ICommandHandler<ReplaceAddressCommand> sut = ReplaceAddressHandler();
        Repository.IsDirty = false;

        await sut.Handle(command);

        Repository.IsDirty.Should().BeTrue();
        var updatedAddress = Repository.First();
        updatedAddress.Street.Should().Be(replacementAddress.Street);
        updatedAddress.City.Should().Be(replacementAddress.City);
    }

    [Fact]
    public async Task Handle_UpdatesMultipleAddresses_Successfully()
    {
        var firstAddress = new AddressTestDataBuilder().Build();
        var secondAddress = new AddressTestDataBuilder().Build();
        Repository.Add(firstAddress);
        Repository.Add(secondAddress);
        var replacementAddress = new AddressTestDataBuilder()
            .WithId(firstAddress.Id)
            .WithStreet(new Street("789 New Street"))
            .WithCity(new City("Another City"))
            .Build();
        var command = new ReplaceAddressCommand { Address = replacementAddress };
        ICommandHandler<ReplaceAddressCommand> sut = ReplaceAddressHandler();
        Repository.IsDirty = false;

        await sut.Handle(command);

        Repository.IsDirty.Should().BeTrue();
        var updatedAddress = Repository.Single(a => a.Id == firstAddress.Id);
        updatedAddress.Id.Should().Be(replacementAddress.Id);
        updatedAddress.Street.Should().Be(replacementAddress.Street);
        updatedAddress.City.Should().Be(replacementAddress.City);
    }
}
