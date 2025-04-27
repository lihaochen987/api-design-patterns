// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Commands.UpdateInventory;
using backend.Inventory.Controllers;
using backend.Inventory.Tests.TestHelpers.Builders;
using backend.Shared.CommandHandler;
using FluentAssertions;
using Xunit;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public class UpdateInventoryHandlerTests : UpdateInventoryHandlerTestBase
{
    [Fact]
    public async Task Handle_WithMultipleFieldsInFieldMask_ShouldUpdateOnlySpecifiedFields()
    {
        var restockDate = Fixture.Create<DateTimeOffset>();
        DomainModels.Inventory inventory = new InventoryTestDataBuilder().Build();
        Repository.Add(inventory);
        Repository.IsDirty = false;
        UpdateInventoryRequest request = new()
        {
            Quantity = 150m,
            RestockDate = restockDate,
            FieldMask = ["quantity", "restockdate"]
        };
        ICommandHandler<UpdateInventoryCommand> sut = UpdateInventoryService();

        await sut.Handle(new UpdateInventoryCommand { Inventory = inventory, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().Quantity.Value.Should().Be(150m);
        Repository.First().RestockDate.Should().Be(restockDate);
    }

    [Fact]
    public async Task Handle_WithPartialFieldMask_ShouldUpdateOnlySpecifiedFields()
    {
        var restockDate = Fixture.Create<DateTimeOffset>();
        DomainModels.Inventory inventory = new InventoryTestDataBuilder()
            .WithRestockDate(restockDate)
            .Build();
        Repository.Add(inventory);
        Repository.IsDirty = false;
        UpdateInventoryRequest request = new()
        {
            Quantity = 150m,
            RestockDate = Fixture.Create<DateTimeOffset>(),
            FieldMask = ["quantity"]
        };
        ICommandHandler<UpdateInventoryCommand> sut = UpdateInventoryService();

        await sut.Handle(new UpdateInventoryCommand { Inventory = inventory, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().Quantity.Value.Should().Be(150m);
        Repository.First().RestockDate.Should().Be(restockDate);
    }

    [Fact]
    public async Task Handle_WithEmptyFieldValues_ShouldNotUpdateFields()
    {
        DomainModels.Inventory inventory = new InventoryTestDataBuilder().Build();
        Repository.Add(inventory);
        Repository.IsDirty = false;
        UpdateInventoryRequest request = new()
        {
            Quantity = null, RestockDate = null, FieldMask = ["quantity", "restockdate"]
        };
        ICommandHandler<UpdateInventoryCommand> sut = UpdateInventoryService();

        await sut.Handle(new UpdateInventoryCommand { Inventory = inventory, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().Should().BeEquivalentTo(inventory);
    }

    [Fact]
    public async Task Handle_WithCaseInsensitiveFieldMask_ShouldUpdateFields()
    {
        var restockDate = Fixture.Create<DateTimeOffset>();
        DomainModels.Inventory inventory = new InventoryTestDataBuilder().Build();
        Repository.Add(inventory);
        Repository.IsDirty = false;
        UpdateInventoryRequest request = new()
        {
            Quantity = 150m,
            RestockDate = restockDate,
            FieldMask = ["QUANTITY", "RESTOCKDATE"]
        };
        ICommandHandler<UpdateInventoryCommand> sut = UpdateInventoryService();

        await sut.Handle(new UpdateInventoryCommand { Inventory = inventory, Request = request });

        Repository.IsDirty.Should().BeTrue();
        Repository.First().Quantity.Value.Should().Be(150m);
        Repository.First().RestockDate.Should().Be(restockDate);
    }
}
