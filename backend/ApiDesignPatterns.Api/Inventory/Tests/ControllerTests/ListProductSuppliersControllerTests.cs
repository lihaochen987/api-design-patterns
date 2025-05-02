// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Queries.GetSuppliersByIds;
using backend.Inventory.ApplicationLayer.Queries.ListInventory;
using backend.Inventory.Controllers;
using backend.Inventory.DomainModels;
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
    public async Task ListProductSuppliers_ShouldReturnAllSuppliers_WhenNoPageTokenProvided()
    {
        long productId = Fixture.Create<long>();
        List<InventoryView> inventoryViews =
            Fixture.Build<InventoryView>().With(x => x.ProductId, productId).CreateMany(4).ToList();
        List<SupplierView> supplierViews = new SupplierViewTestDataBuilder()
            .CreateMany(4)
            .ToList();
        for (int i = 0; i < inventoryViews.Count; i++)
        {
            inventoryViews[i].SupplierId = supplierViews[i].Id;
        }

        ListProductSuppliersRequest request = new() { MaxPageSize = 4 };
        Mock
            .Get(MockListInventory)
            .Setup(svc => svc.Handle(It.Is<ListInventoryQuery>(q =>
                q.Filter == $"ProductId == {productId}" &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedInventory(inventoryViews, null));
        var supplierIds = inventoryViews.Select(x => x.SupplierId).ToList();
        Mock
            .Get(MockGetSuppliersByIds)
            .Setup(svc => svc.Handle(It.Is<GetSuppliersByIdsQuery>(q =>
                q.SupplierIds.SequenceEqual(supplierIds))))
            .ReturnsAsync(supplierViews);
        ListProductSuppliersController sut = ListProductSuppliersController();

        var result = await sut.ListProductSuppliers(request, productId);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listProductSuppliersResponse = (ListProductSuppliersResponse)response.Value!;
        listProductSuppliersResponse.Results.Count().Should().Be(4);
        listProductSuppliersResponse.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task ListProductSuppliers_ShouldReturnSuppliersAfterPageToken_WhenPageTokenProvided()
    {
        long productId = Fixture.Create<long>();
        List<InventoryView> inventoryViews =
            Fixture.Build<InventoryView>().With(x => x.ProductId, productId).CreateMany(2).ToList();
        List<SupplierView> supplierViews = new SupplierViewTestDataBuilder()
            .CreateMany(2)
            .ToList();
        for (int i = 0; i < inventoryViews.Count; i++)
        {
            inventoryViews[i].SupplierId = supplierViews[i].Id;
        }

        ListProductSuppliersRequest request = new() { PageToken = "2", MaxPageSize = 2 };
        Mock
            .Get(MockListInventory)
            .Setup(svc => svc.Handle(It.Is<ListInventoryQuery>(q =>
                q.Filter == $"ProductId == {productId}" &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedInventory(inventoryViews, null));

        var supplierIds = inventoryViews.Select(x => x.SupplierId).ToList();
        Mock
            .Get(MockGetSuppliersByIds)
            .Setup(svc => svc.Handle(It.Is<GetSuppliersByIdsQuery>(q =>
                q.SupplierIds.SequenceEqual(supplierIds))))
            .ReturnsAsync(supplierViews);

        ListProductSuppliersController sut = ListProductSuppliersController();

        var result = await sut.ListProductSuppliers(request, productId);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listProductSuppliersResponse = (ListProductSuppliersResponse)response.Value!;
        listProductSuppliersResponse.Results.Count.Should().Be(2);
        listProductSuppliersResponse.NextPageToken.Should().BeNull();
    }

    [Fact]
    public async Task ListProductSuppliers_ShouldReturnNextPageToken_WhenMoreSuppliersExist()
    {
        long productId = Fixture.Create<long>();
        List<InventoryView> inventoryViews =
            Fixture.Build<InventoryView>().With(x => x.ProductId, productId).CreateMany(2).ToList();
        List<SupplierView> supplierViews = new SupplierViewTestDataBuilder()
            .CreateMany(2)
            .ToList();
        for (int i = 0; i < inventoryViews.Count; i++)
        {
            inventoryViews[i].SupplierId = supplierViews[i].Id;
        }

        ListProductSuppliersRequest request = new() { MaxPageSize = 2 };
        Mock
            .Get(MockListInventory)
            .Setup(svc => svc.Handle(It.Is<ListInventoryQuery>(q =>
                q.Filter == $"ProductId == {productId}" &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedInventory(inventoryViews, "2"));
        var supplierIds = inventoryViews.Select(x => x.SupplierId).ToList();
        Mock
            .Get(MockGetSuppliersByIds)
            .Setup(svc => svc.Handle(It.Is<GetSuppliersByIdsQuery>(q =>
                q.SupplierIds.SequenceEqual(supplierIds))))
            .ReturnsAsync(supplierViews);
        ListProductSuppliersController sut = ListProductSuppliersController();

        var result = await sut.ListProductSuppliers(request, productId);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listProductSuppliersResponse = (ListProductSuppliersResponse)response.Value!;
        listProductSuppliersResponse.Results.Count.Should().Be(2);
        listProductSuppliersResponse.NextPageToken.Should().BeEquivalentTo("2");
    }

    [Fact]
    public async Task ListProductSuppliers_ShouldReturnEmptyList_WhenNoSuppliersExist()
    {
        long productId = Fixture.Create<long>();
        List<InventoryView> inventoryViews = [];
        List<SupplierView> supplierViews = [];
        ListProductSuppliersRequest request = new() { MaxPageSize = 2 };
        Mock
            .Get(MockListInventory)
            .Setup(svc => svc.Handle(It.Is<ListInventoryQuery>(q =>
                q.Filter == $"ProductId == {productId}" &&
                q.MaxPageSize == request.MaxPageSize &&
                q.PageToken == request.PageToken)))
            .ReturnsAsync(new PagedInventory(inventoryViews, null));
        var supplierIds = inventoryViews.Select(x => x.SupplierId).ToList();
        Mock
            .Get(MockGetSuppliersByIds)
            .Setup(svc => svc.Handle(It.Is<GetSuppliersByIdsQuery>(q =>
                q.SupplierIds.SequenceEqual(supplierIds))))
            .ReturnsAsync(supplierViews);
        ListProductSuppliersController sut = ListProductSuppliersController();

        var result = await sut.ListProductSuppliers(request, productId);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        var response = (OkObjectResult)result.Result;
        response.Should().NotBeNull();
        var listProductSuppliersResponse = (ListProductSuppliersResponse)response.Value!;
        listProductSuppliersResponse.Results.Should().BeEmpty();
        listProductSuppliersResponse.NextPageToken.Should().BeNull();
    }
}
