// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.ApplicationLayer.Queries.GetInventoryById;
using backend.Inventory.Tests.TestHelpers.Builders;
using backend.Shared.QueryHandler;
using FluentAssertions;
using Xunit;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public class GetInventoryHandlerTests : GetInventoryHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsInventory_WhenInventoryExists()
    {
        DomainModels.Inventory expectedInventory = new InventoryTestDataBuilder().Build();
        Repository.Add(expectedInventory);
        IQueryHandler<GetInventoryQuery, DomainModels.Inventory?> sut = GetInventoryHandler();

        DomainModels.Inventory? result = await sut.Handle(new GetInventoryQuery { Id = expectedInventory.Id });

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedInventory);
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenInventoryDoesNotExist()
    {
        DomainModels.Inventory nonExistentInventory = new InventoryTestDataBuilder().Build();
        IQueryHandler<GetInventoryQuery, DomainModels.Inventory?> sut = GetInventoryHandler();

        DomainModels.Inventory? result = await sut.Handle(new GetInventoryQuery { Id = nonExistentInventory.Id });

        result.Should().BeNull();
    }
}
