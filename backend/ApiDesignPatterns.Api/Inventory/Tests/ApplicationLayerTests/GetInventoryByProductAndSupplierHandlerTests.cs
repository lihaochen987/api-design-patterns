// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryByProductAndSupplier;
using backend.Inventory.Tests.TestHelpers.Builders;
using backend.Shared.QueryHandler;
using FluentAssertions;
using Xunit;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public class GetInventoryByProductAndSupplierHandlerTests : GetInventoryByProductAndSupplierHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsInventory_WhenMatchingInventoryExists()
    {
        var expectedInventory = new InventoryTestDataBuilder().Build();
        Repository.Add(expectedInventory);
        IQueryHandler<GetInventoryByProductAndSupplierQuery, DomainModels.Inventory?> sut = GetHandler();
        var query = new GetInventoryByProductAndSupplierQuery
        {
            ProductId = expectedInventory.ProductId, SupplierId = expectedInventory.SupplierId
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
        IQueryHandler<GetInventoryByProductAndSupplierQuery, DomainModels.Inventory?> sut = GetHandler();
        var query = new GetInventoryByProductAndSupplierQuery
        {
            ProductId = Fixture.Create<long>(), SupplierId = Fixture.Create<long>()
        };

        DomainModels.Inventory? result = await sut.Handle(query);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenOnlyProductIdMatches()
    {
        var existingInventory = new InventoryTestDataBuilder().Build();
        Repository.Add(existingInventory);
        IQueryHandler<GetInventoryByProductAndSupplierQuery, DomainModels.Inventory?> sut = GetHandler();
        var query = new GetInventoryByProductAndSupplierQuery
        {
            ProductId = existingInventory.ProductId, SupplierId = Fixture.Create<long>()
        };

        DomainModels.Inventory? result = await sut.Handle(query);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenOnlySupplierIdMatches()
    {
        var existingInventory = new InventoryTestDataBuilder().Build();
        Repository.Add(existingInventory);
        IQueryHandler<GetInventoryByProductAndSupplierQuery, DomainModels.Inventory?> sut = GetHandler();
        var query = new GetInventoryByProductAndSupplierQuery
        {
            ProductId = Fixture.Create<long>(), SupplierId = existingInventory.SupplierId
        };

        DomainModels.Inventory? result = await sut.Handle(query);

        result.Should().BeNull();
    }
}
