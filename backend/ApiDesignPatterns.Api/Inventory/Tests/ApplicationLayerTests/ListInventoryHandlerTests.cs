// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.ApplicationLayer.Queries.ListInventory;
using backend.Inventory.Controllers;
using backend.Inventory.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public class ListInventoryHandlerTests : ListInventoryHandlerTestBase
{
    [Fact]
    public async Task Handle_ShouldReturnInventoryAndNullToken_WhenDataFitsPage()
    {
        const int inventoryCount = 3;
        Repository.AddInventoryView(inventoryCount);
        var request = new ListInventoryRequest { MaxPageSize = 5 };
        var query = new ListInventoryQuery { Request = request };
        var sut = ListInventoryViewHandler();

        var result = await sut.Handle(query);

        result.Inventory.Should().HaveCount(inventoryCount);
    }

    [Fact]
    public async Task Handle_ShouldReturnInventoryAndNextPageToken_WhenMoreDataExists()
    {
        const int totalInventory = 7;
        const int pageSize = 5;
        Repository.AddInventoryView(totalInventory);
        var request = new ListInventoryRequest { MaxPageSize = pageSize };
        var query = new ListInventoryQuery { Request = request };
        var sut = ListInventoryViewHandler();

        var result = await sut.Handle(query);

        result.Inventory.Should().HaveCount(pageSize);
        result.NextPageToken.Should().Be(Repository.ElementAt(pageSize - 1).Id.ToString());
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoInventoryExists()
    {
        var request = new ListInventoryRequest();
        var query = new ListInventoryQuery { Request = request };
        var sut = ListInventoryViewHandler();

        var result = await sut.Handle(query);

        result.Inventory.Should().BeEmpty();
        result.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldPassParametersToRepositoryCorrectly()
    {
        var inventory = new InventoryViewTestDataBuilder().WithProductId(987).Build();
        Repository.Add(inventory);
        var request = new ListInventoryRequest { Filter = "ProductId == 987", PageToken = "5", MaxPageSize = 15 };
        var query = new ListInventoryQuery { Request = request };
        var sut = ListInventoryViewHandler();

        var result = await sut.Handle(query);

        result.Inventory.Should().HaveCount(1);
        result.Inventory.First().ProductId.Should().Be(987);
    }
}
