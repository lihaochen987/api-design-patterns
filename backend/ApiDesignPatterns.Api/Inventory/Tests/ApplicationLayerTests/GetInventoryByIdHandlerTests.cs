// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.ApplicationLayer.Queries.GetInventoryById;
using backend.Inventory.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public class GetInventoryByIdByIdHandlerTests : GetInventoryByIdHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsInventory_WhenInventoryExists()
    {
        DomainModels.Inventory expectedInventory = new InventoryTestDataBuilder().Build();
        Repository.Add(expectedInventory);
        var sut = GetInventoryHandler();

        DomainModels.Inventory? result = await sut.Handle(new GetInventoryByIdQuery { Id = expectedInventory.Id });

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedInventory);
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenInventoryDoesNotExist()
    {
        DomainModels.Inventory nonExistentInventory = new InventoryTestDataBuilder().Build();
        var sut = GetInventoryHandler();

        DomainModels.Inventory? result = await sut.Handle(new GetInventoryByIdQuery { Id = nonExistentInventory.Id });

        result.Should().BeNull();
    }
}
