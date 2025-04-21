// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Queries.ListInventory;
using backend.Inventory.Controllers;
using backend.Inventory.DomainModels;
using backend.Inventory.Tests.TestHelpers.Builders;
using backend.Supplier.ApplicationLayer.Queries.GetSupplierView;
using backend.Supplier.DomainModels;
using backend.Supplier.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace backend.Inventory.Tests.ControllerTests;

public class ListProductSuppliersControllerTests : ListProductSuppliersControllerTestBase
{
    [Fact]
    public async Task ListInventory_ShouldReturnAllSuppliers_WhenNoPageTokenProvided()
    {
        decimal productId = Fixture.Create<decimal>();
        List<InventoryView> inventoryViews = new InventoryViewTestDataBuilder().CreateMany(4).ToList();
        List<SupplierView> supplierViews = new SupplierViewTestDataBuilder().CreateMany(4).ToList();
        AssociateInventoryWithSuppliers(inventoryViews, supplierViews);
        ListProductSuppliersRequest request = new() { MaxPageSize = 4 };
        Mock
            .Get(MockListInventory)
            .Setup(svc => svc.Handle(It.Is<ListInventoryQuery>(q =>
                q.Filter == $"ProductId == {productId}" &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedInventory(inventoryViews, null));
        SetupSupplierViewMocks(supplierViews);
        ListProductSuppliersController sut = ListProductSuppliersController();

        var result = await sut.ListInventory(request, productId);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listSuppliersResponse = (ListProductSuppliersResponse)response.Value!;
        listSuppliersResponse.Results.Count().Should().Be(4);
        listSuppliersResponse.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task ListInventory_ShouldReturnSuppliersAfterPageToken_WhenPageTokenProvided()
    {
        decimal productId = Fixture.Create<decimal>();
        List<InventoryView> inventoryViews = new InventoryViewTestDataBuilder().CreateMany(2).ToList();
        List<SupplierView> supplierViews = new SupplierViewTestDataBuilder().CreateMany(2).ToList();
        AssociateInventoryWithSuppliers(inventoryViews, supplierViews);
        ListProductSuppliersRequest request = new() { PageToken = "2", MaxPageSize = 2 };
        Mock
            .Get(MockListInventory)
            .Setup(svc => svc.Handle(It.Is<ListInventoryQuery>(q =>
                q.Filter == $"ProductId == {productId}" &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedInventory(inventoryViews, null));
        SetupSupplierViewMocks(supplierViews);
        ListProductSuppliersController sut = ListProductSuppliersController();

        var result = await sut.ListInventory(request, productId);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listSuppliersResponse = (ListProductSuppliersResponse)response.Value!;
        listSuppliersResponse.Results.Count().Should().Be(2);
        listSuppliersResponse.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task ListInventory_ShouldReturnNextPageToken_WhenMoreInventoryItemsExist()
    {
        decimal productId = Fixture.Create<decimal>();
        List<InventoryView> inventoryViews = new InventoryViewTestDataBuilder().CreateMany(2).ToList();
        List<SupplierView> supplierViews = new SupplierViewTestDataBuilder().CreateMany(2).ToList();
        AssociateInventoryWithSuppliers(inventoryViews, supplierViews);
        ListProductSuppliersRequest request = new() { MaxPageSize = 2 };
        Mock
            .Get(MockListInventory)
            .Setup(svc => svc.Handle(It.Is<ListInventoryQuery>(q =>
                q.Filter == $"ProductId == {productId}" &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedInventory(inventoryViews, "2"));
        SetupSupplierViewMocks(supplierViews);
        ListProductSuppliersController sut = ListProductSuppliersController();

        var result = await sut.ListInventory(request, productId);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listSuppliersResponse = (ListProductSuppliersResponse)response.Value!;
        listSuppliersResponse.Results.Count().Should().Be(2);
        listSuppliersResponse.NextPageToken.Should().BeEquivalentTo("2");
    }

    [Fact]
    public async Task ListInventory_ShouldUseDefaults_WhenPageTokenAndMaxPageSizeNotProvided()
    {
        decimal productId = Fixture.Create<decimal>();
        List<InventoryView> inventoryViews = new InventoryViewTestDataBuilder().CreateMany(DefaultMaxPageSize).ToList();
        List<SupplierView> supplierViews = new SupplierViewTestDataBuilder().CreateMany(DefaultMaxPageSize).ToList();
        AssociateInventoryWithSuppliers(inventoryViews, supplierViews);
        ListProductSuppliersRequest request = new();
        Mock
            .Get(MockListInventory)
            .Setup(svc => svc.Handle(It.Is<ListInventoryQuery>(q =>
                q.Filter == $"ProductId == {productId}" &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedInventory(inventoryViews, DefaultMaxPageSize.ToString()));
        SetupSupplierViewMocks(supplierViews);
        ListProductSuppliersController sut = ListProductSuppliersController();

        var result = await sut.ListInventory(request, productId);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listSuppliersResponse = (ListProductSuppliersResponse)response.Value!;
        listSuppliersResponse.Results.Count().Should().Be(DefaultMaxPageSize);
        listSuppliersResponse.NextPageToken.Should().BeEquivalentTo(DefaultMaxPageSize.ToString());
    }

    [Fact]
    public async Task ListInventory_ShouldReturnEmptyList_WhenNoInventoryItemsExist()
    {
        decimal productId = Fixture.Create<decimal>();
        ListProductSuppliersRequest request = new() { MaxPageSize = 2 };
        Mock
            .Get(MockListInventory)
            .Setup(svc => svc.Handle(It.Is<ListInventoryQuery>(q =>
                q.Filter == $"ProductId == {productId}" &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedInventory([], null));
        ListProductSuppliersController sut = ListProductSuppliersController();

        var result = await sut.ListInventory(request, productId);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listSuppliersResponse = (ListProductSuppliersResponse)response.Value!;
        listSuppliersResponse.Results.Should().BeEmpty();
        listSuppliersResponse.NextPageToken.Should().BeNull();
    }

    private static void AssociateInventoryWithSuppliers(List<InventoryView> inventoryViews, List<SupplierView> supplierViews)
    {
        for (int i = 0; i < inventoryViews.Count; i++)
        {
            inventoryViews[i] = inventoryViews[i] with { SupplierId = supplierViews[i].Id };
        }
    }

    private  void SetupSupplierViewMocks(List<SupplierView> supplierViews)
    {
        foreach (var supplierView in supplierViews)
        {
            long supplierId = supplierView.Id;
            Mock
                .Get(MockGetSupplierView)
                .Setup(svc => svc.Handle(It.Is<GetSupplierViewQuery>(q => q.Id == supplierId)))
                .ReturnsAsync(supplierView);
        }
    }
}
