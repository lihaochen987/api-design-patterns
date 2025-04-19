// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Commands.DeleteInventory;
using backend.Inventory.Tests.TestHelpers.Builders;
using backend.Shared.CommandHandler;
using FluentAssertions;
using Xunit;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public class DeleteInventoryCommandHandlerTests : DeleteInventoryCommandHandlerTestBase
{
    [Fact]
    public async Task Handle_DeletesInventory_WhenInventoryExists()
    {
        DomainModels.Inventory inventoryToDelete = new InventoryTestDataBuilder().Build();
        Repository.Add(inventoryToDelete);
        ICommandHandler<DeleteInventoryCommand> sut = DeleteInventoryHandler();
        var command = new DeleteInventoryCommand { Id = inventoryToDelete.Id };

        await sut.Handle(command);

        Repository.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_DoesNotThrow_WhenInventoryDoesNotExist()
    {
        ICommandHandler<DeleteInventoryCommand> sut = DeleteInventoryHandler();
        var command = new DeleteInventoryCommand { Id = Fixture.Create<long>() };
        Repository.Should().BeEmpty();

        Func<Task> act = async () => await sut.Handle(command);

        await act.Should().NotThrowAsync();
        Repository.Should().BeEmpty();
        Repository.IsDirty.Should().BeFalse();
    }
}
