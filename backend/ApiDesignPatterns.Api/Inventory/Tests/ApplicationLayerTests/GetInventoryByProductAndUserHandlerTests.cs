// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryByProductAndUser;
using backend.Inventory.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public class GetInventoryByProductAndUserHandlerTests : GetInventoryByProductAndUserHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsInventory_WhenMatchingInventoryExists()
    {
        var expectedInventory = new InventoryTestDataBuilder().Build();
        Repository.Add(expectedInventory);
        var sut = GetHandler();
        var query = new GetInventoryByProductAndUserQuery
        {
            ProductId = expectedInventory.ProductId, UserId = expectedInventory.UserId
        };

        DomainModels.Inventory? result = await sut.Handle(query);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedInventory);
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenNoMatchingInventoryExists()
    {
        var otherInventory = new InventoryTestDataBuilder().Build();
        Repository.Add(otherInventory);
        var sut = GetHandler();
        var query = new GetInventoryByProductAndUserQuery
        {
            ProductId = Fixture.Create<long>(), UserId = Fixture.Create<long>()
        };

        DomainModels.Inventory? result = await sut.Handle(query);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenOnlyProductIdMatches()
    {
        var existingInventory = new InventoryTestDataBuilder().Build();
        Repository.Add(existingInventory);
        var sut = GetHandler();
        var query = new GetInventoryByProductAndUserQuery
        {
            ProductId = existingInventory.ProductId, UserId = Fixture.Create<long>()
        };

        DomainModels.Inventory? result = await sut.Handle(query);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenOnlyUserIdMatches()
    {
        var existingInventory = new InventoryTestDataBuilder().Build();
        Repository.Add(existingInventory);
        var sut = GetHandler();
        var query = new GetInventoryByProductAndUserQuery
        {
            ProductId = Fixture.Create<long>(), UserId = existingInventory.UserId
        };

        DomainModels.Inventory? result = await sut.Handle(query);

        result.Should().BeNull();
    }
}
