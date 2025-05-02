// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.ApplicationLayer.Queries.ListInventory;
using backend.Inventory.Controllers;
using backend.Inventory.DomainModels;
using backend.Inventory.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace backend.Inventory.Tests.ControllerTests;

public class ListInventoryControllerTests : ListInventoryControllerTestBase
{
    [Fact]
    public async Task ListInventory_ShouldReturnAllInventory_WhenNoPageTokenProvided()
    {
        List<InventoryView> inventoryViews = new InventoryViewTestDataBuilder().CreateMany(4).ToList();
        ListInventoryRequest request = new() { MaxPageSize = 4 };
        Mock
            .Get(MockListInventory)
            .Setup(svc => svc.Handle(It.Is<ListInventoryQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedInventory(inventoryViews, null));
        ListInventoryController sut = ListInventoryController();

        var result = await sut.ListInventory(request);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listInventoryResponse = (ListInventoryResponse)response.Value!;
        listInventoryResponse.Results.Count().Should().Be(4);
        listInventoryResponse.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task ListInventory_ShouldReturnInventoryAfterPageToken_WhenPageTokenProvided()
    {
        List<InventoryView> inventoryViewList = new InventoryViewTestDataBuilder().CreateMany(4).ToList();
        var expectedPageResults = inventoryViewList.Skip(2).Take(2).ToList();
        ListInventoryRequest request = new() { PageToken = "2", MaxPageSize = 2 };
        Mock
            .Get(MockListInventory)
            .Setup(svc => svc.Handle(It.Is<ListInventoryQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedInventory(expectedPageResults, null));
        ListInventoryController sut = ListInventoryController();

        var result = await sut.ListInventory(request);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listInventoryResponse = (ListInventoryResponse)response.Value!;
        listInventoryResponse.Results.Count().Should().Be(2);
        listInventoryResponse.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task ListInventory_ShouldReturnNextPageToken_WhenMoreInventoryExists()
    {
        List<InventoryView> inventory = new InventoryViewTestDataBuilder().CreateMany(20).ToList();
        List<InventoryView> firstPageInventory = inventory.Take(2).ToList();
        ListInventoryRequest request = new() { MaxPageSize = 2 };
        Mock
            .Get(MockListInventory)
            .Setup(svc => svc.Handle(It.Is<ListInventoryQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedInventory(firstPageInventory, "2"));
        ListInventoryController sut = ListInventoryController();

        var result = await sut.ListInventory(request);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listInventoryResponse = (ListInventoryResponse)response.Value!;
        listInventoryResponse.Results.Count().Should().Be(2);
        listInventoryResponse.NextPageToken.Should().BeEquivalentTo("2");
    }

    [Fact]
    public async Task ListInventory_ShouldUseDefaults_WhenPageTokenAndMaxPageSizeNotProvided()
    {
        List<InventoryView> inventory = new InventoryViewTestDataBuilder().CreateMany(20).ToList();
        List<InventoryView> defaultPageInventory = inventory.Take(DefaultMaxPageSize).ToList();
        ListInventoryRequest request = new();
        Mock
            .Get(MockListInventory)
            .Setup(svc => svc.Handle(It.Is<ListInventoryQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedInventory(defaultPageInventory, DefaultMaxPageSize.ToString()));
        ListInventoryController sut = ListInventoryController();

        var result = await sut.ListInventory(request);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listInventoryResponse = (ListInventoryResponse)response.Value!;
        listInventoryResponse.Results.Count().Should().Be(DefaultMaxPageSize);
        listInventoryResponse.NextPageToken.Should().BeEquivalentTo(DefaultMaxPageSize.ToString());
    }

    [Fact]
    public async Task ListInventory_ShouldReturnEmptyList_WhenNoInventoryExists()
    {
        ListInventoryRequest request = new() { MaxPageSize = 2 };
        Mock
            .Get(MockListInventory)
            .Setup(svc => svc.Handle(It.Is<ListInventoryQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedInventory([], null));
        ListInventoryController sut = ListInventoryController();

        var result = await sut.ListInventory(request);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listInventoryResponse = (ListInventoryResponse)response.Value!;
        listInventoryResponse.Results.Should().BeEmpty();
        listInventoryResponse.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task ListInventory_WithFilterAndPagination_ReturnsCorrectResults()
    {
        var inventory = new InventoryViewTestDataBuilder()
            .WithId(2)
            .WithQuantity(50)
            .Build();
        var filteredInventory = new List<InventoryView> { inventory };
        ListInventoryRequest request = new() { Filter = "Quantity == 50", MaxPageSize = 2, PageToken = "1" };
        Mock
            .Get(MockListInventory)
            .Setup(svc => svc.Handle(It.Is<ListInventoryQuery>(q =>
                q.Filter == request.Filter &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedInventory(filteredInventory, "2"));
        ListInventoryController sut = ListInventoryController();

        var result = await sut.ListInventory(request);

        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        var listInventoryResponse = (ListInventoryResponse)response.Value!;
        listInventoryResponse.Results.Count().Should().Be(1);
        listInventoryResponse.NextPageToken.Should().Be("2");
    }
}
