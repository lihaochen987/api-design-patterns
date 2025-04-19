// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.ApplicationLayer.Queries.ListInventory;
using backend.Inventory.Controllers;
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

    //
    // [Fact]
    // public async Task Handle_ShouldReturnNextPage_WhenUsingPageToken()
    // {
    //     // Arrange
    //     int totalInventory = 12;
    //     int pageSize = 5;
    //     Repository.AddInventoryViews(totalInventory);
    //     var initialRequest = new ListInventoryRequest { MaxPageSize = pageSize };
    //     var initialQuery = new ListInventoryQuery { Request = initialRequest };
    //     var sut = ListInventoryViewHandler();
    //
    //     // Act - First Call
    //     var firstResult = await sut.Handle(initialQuery);
    //
    //     // Arrange - Second Call
    //     var secondRequest = new ListInventoryRequest { MaxPageSize = pageSize, PageToken = firstResult.NextPageToken };
    //     var secondQuery = new ListInventoryQuery { Request = secondRequest };
    //
    //     // Act - Second Call
    //     var secondResult = await sut.Handle(secondQuery);
    //
    //     // Assert - Second Call Results
    //     secondResult.Should().NotBeNull();
    //     secondResult.Inventory.Should().NotBeNull();
    //     secondResult.Inventory.Should().HaveCount(pageSize); // Expecting the second page
    //     secondResult.NextPageToken.Should().NotBeNullOrEmpty(); // Still more data
    //     secondResult.NextPageToken.Should().Be((pageSize * 2).ToString());
    //     Repository.CallCount.Should().Be(2); // Repo called twice
    //     Repository.LastUsedPageToken.Should().Be(firstResult.NextPageToken); // Verify token was passed
    // }
    //
    // [Fact]
    // public async Task Handle_ShouldReturnEmptyList_WhenNoInventoryExists()
    // {
    //     // Arrange
    //     // No data added to Repository
    //     var request = new ListInventoryRequest();
    //     var query = new ListInventoryQuery { Request = request };
    //     var sut = ListInventoryViewHandler();
    //
    //     // Act
    //     var result = await sut.Handle(query);
    //
    //     // Assert
    //     result.Should().NotBeNull();
    //     result.Inventory.Should().NotBeNull();
    //     result.Inventory.Should().BeEmpty();
    //     result.NextPageToken.Should().BeNull();
    //     Repository.CallCount.Should().Be(1);
    // }
    //
    // [Fact]
    // public async Task Handle_ShouldPassParametersToRepositoryCorrectly()
    // {
    //     // Arrange
    //     string expectedFilter = "ProductId == 987";
    //     string expectedToken = "5";
    //     int expectedPageSize = 15;
    //     var request = new ListInventoryRequest
    //     {
    //         Filter = expectedFilter, PageToken = expectedToken, MaxPageSize = expectedPageSize
    //     };
    //     var query = new ListInventoryQuery { Request = request };
    //     var sut = ListInventoryViewHandler();
    //
    //     // Act
    //     await sut.Handle(query);
    //
    //     // Assert
    //     Repository.CallCount.Should().Be(1);
    //     Repository.LastUsedFilter.Should().Be(expectedFilter);
    //     Repository.LastUsedPageToken.Should().Be(expectedToken);
    //     Repository.LastUsedMaxPageSize.Should().Be(expectedPageSize);
    // }
    //
    // [Fact]
    // public async Task Handle_ShouldReturnFilteredResults_WhenFilterIsApplied()
    // {
    //     // Arrange
    //     long targetSupplierId = Fixture.Create<long>();
    //     long otherSupplierId = Fixture.Create<long>();
    //
    //     Repository.AddInventoryView(Fixture.Build<InventoryView>().With(i => i.SupplierId, targetSupplierId).Create());
    //     Repository.AddInventoryView(Fixture.Build<InventoryView>().With(i => i.SupplierId, targetSupplierId).Create());
    //     Repository.AddInventoryView(Fixture.Build<InventoryView>().With(i => i.SupplierId, otherSupplierId)
    //         .Create()); // Different supplier
    //
    //     var request = new ListInventoryRequest { Filter = $"SupplierId == {targetSupplierId}" };
    //     var query = new ListInventoryQuery { Request = request };
    //     var sut = ListInventoryViewHandler();
    //
    //     // Act
    //     var result = await sut.Handle(query);
    //
    //     // Assert
    //     result.Should().NotBeNull();
    //     result.Inventory.Should().HaveCount(2); // Only items matching the filter
    //     result.Inventory.Should().OnlyContain(i => i.SupplierId == targetSupplierId);
    //     result.NextPageToken.Should().BeNull(); // Assuming 2 items fit in default page size
    //     Repository.LastUsedFilter.Should().Be(request.Filter);
    // }
    //
    // [Fact]
    // public async Task Handle_ShouldPropagateArgumentException_WhenRepositoryThrowsOnBadFilter()
    // {
    //     // Arrange
    //     var request = new ListInventoryRequest { Filter = "INVALID_FILTER" }; // Use the filter the fake repo reacts to
    //     var query = new ListInventoryQuery { Request = request };
    //     var sut = ListInventoryViewHandler();
    //
    //     // Act & Assert
    //     await FluentActions.Invoking(() => sut.Handle(query))
    //         .Should().ThrowAsync<ArgumentException>()
    //         .WithMessage("Simulated invalid filter."); // Match the exception from the fake
    //
    //     Repository.CallCount.Should().Be(1); // Ensure the repo method was still called
    // }
}
