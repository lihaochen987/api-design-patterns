// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.ApplicationLayer.Commands.CreateInventory;
using backend.Inventory.Tests.TestHelpers.Builders;
using backend.Shared.CommandHandler;
using FluentAssertions;
using Xunit;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public class CreateInventoryHandlerTests : CreateInventoryHandlerTestBase
{
    [Fact]
    public async Task Handle_CallsRepositoryWithCorrectInventory()
    {
        var inventoryToCreate = new InventoryTestDataBuilder().Build();
        ICommandHandler<CreateInventoryCommand> sut = CreateInventoryService();

        await sut.Handle(new CreateInventoryCommand { Inventory = inventoryToCreate });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().Should().BeEquivalentTo(inventoryToCreate);
    }

    [Fact]
    public async Task Handle_PersistsWhenCalledTwice()
    {
        var firstInventoryToCreate = new InventoryTestDataBuilder().Build();
        var secondInventoryToCreate = new InventoryTestDataBuilder().Build();
        ICommandHandler<CreateInventoryCommand> sut = CreateInventoryService();

        await sut.Handle(new CreateInventoryCommand { Inventory = firstInventoryToCreate });
        await sut.Handle(new CreateInventoryCommand { Inventory = secondInventoryToCreate });

        Repository.IsDirty.Should().BeTrue();
        var firstInventory = Repository.First(x => x.Id == firstInventoryToCreate.Id);
        firstInventory.Should().BeEquivalentTo(firstInventoryToCreate);
        var secondInventory = Repository.First(x => x.Id == secondInventoryToCreate.Id);
        secondInventory.Should().BeEquivalentTo(secondInventoryToCreate);
    }
}
