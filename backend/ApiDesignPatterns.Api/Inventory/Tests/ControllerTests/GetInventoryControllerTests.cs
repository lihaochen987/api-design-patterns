// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using System.Net;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryView;
using backend.Inventory.DomainModels;
using backend.Inventory.InventoryControllers;
using backend.Inventory.Tests.TestHelpers.Builders;
using backend.Product.ProductControllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace backend.Inventory.Tests.ControllerTests;

public class GetInventoryControllerTests : GetInventoryControllerTestBase
{
    [Fact]
    public async Task GetInventory_ReturnsOkResult_WhenInventoryExists()
    {
        long inventoryId = Fixture.Create<long>();
        var inventoryView = new InventoryViewTestDataBuilder()
            .WithId(inventoryId)
            .WithQuantity(100)
            .WithSupplierId(42)
            .Build();
        var request = Fixture.Build<GetInventoryRequest>()
            .With(r => r.FieldMask, ["Quantity", "SupplierId"])
            .Create();
        Mock
            .Get(MockGetInventoryView)
            .Setup(service => service.Handle(It.Is<GetInventoryViewQuery>(q => q.Id == inventoryId)))
            .ReturnsAsync(inventoryView);
        GetInventoryController sut = GetInventoryController();

        ActionResult<GetProductResponse> result = await sut.GetInventory(inventoryId, request);

        OkObjectResult okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        okResult.Value.ShouldNotBeNull();
        string jsonResult = (okResult.Value as string)!;
        jsonResult.ShouldContain("100");
        jsonResult.ShouldContain("42");
    }

    [Fact]
    public async Task GetInventory_ReturnsNotFound_WhenInventoryDoesNotExist()
    {
        long inventoryId = Fixture.Create<long>();
        var request = Fixture.Create<GetInventoryRequest>();
        Mock
            .Get(MockGetInventoryView)
            .Setup(service => service.Handle(It.Is<GetInventoryViewQuery>(q => q.Id == inventoryId)))
            .ReturnsAsync((InventoryView?)null);
        GetInventoryController sut = GetInventoryController();

        ActionResult<GetProductResponse> result = await sut.GetInventory(inventoryId, request);

        result.Result.ShouldBeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetInventory_SerializesWithFieldMaskCorrectly()
    {
        long inventoryId = Fixture.Create<long>();
        var inventoryView = new InventoryViewTestDataBuilder()
            .WithId(inventoryId)
            .WithQuantity(100)
            .WithSupplierId(42)
            .WithProductId(24)
            .Build();
        var request = Fixture.Build<GetInventoryRequest>()
            .With(r => r.FieldMask, ["Quantity"])
            .Create();
        Mock
            .Get(MockGetInventoryView)
            .Setup(service => service.Handle(It.Is<GetInventoryViewQuery>(q => q.Id == inventoryId)))
            .ReturnsAsync(inventoryView);
        GetInventoryController sut = GetInventoryController();

        ActionResult<GetProductResponse> result = await sut.GetInventory(inventoryId, request);

        OkObjectResult okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe((int)HttpStatusCode.OK);
        okResult.Value.ShouldNotBeNull();
        string jsonResult = (okResult.Value as string)!;
        jsonResult.ShouldContain("100");
        jsonResult.ShouldNotContain("42");
        jsonResult.ShouldNotContain("24");
    }

    [Fact]
    public async Task GetInventory_HandlesQueryCorrectly()
    {
        long inventoryId = Fixture.Create<long>();
        var request = Fixture.Create<GetInventoryRequest>();
        GetInventoryController sut = GetInventoryController();

        await sut.GetInventory(inventoryId, request);

        Mock
            .Get(MockGetInventoryView)
            .Verify(x => x.Handle(It.Is<GetInventoryViewQuery>(q =>
                q.Id == inventoryId)), Times.Once);
    }
}
