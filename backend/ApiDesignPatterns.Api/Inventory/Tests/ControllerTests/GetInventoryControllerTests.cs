// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using System.Net;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryView;
using backend.Inventory.Controllers;
using backend.Inventory.DomainModels;
using backend.Inventory.Tests.TestHelpers.Builders;
using backend.Product.Controllers.Product;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
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
            .WithUserId(42)
            .Build();
        var request = Fixture.Build<GetInventoryRequest>()
            .With(r => r.FieldMask, ["Quantity", "UserId"])
            .Create();
        Mock
            .Get(MockGetInventoryView)
            .Setup(service => service.Handle(It.Is<GetInventoryViewQuery>(q => q.Id == inventoryId)))
            .ReturnsAsync(inventoryView);
        GetInventoryController sut = GetInventoryController();

        ActionResult<GetProductResponse> result = await sut.GetInventory(inventoryId, request);

        OkObjectResult okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().NotBeNull();
        string jsonResult = (string)okResult.Value;
        jsonResult.Should().Contain("100");
        jsonResult.Should().Contain("42");
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

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetInventory_SerializesWithFieldMaskCorrectly()
    {
        long inventoryId = Fixture.Create<long>();
        var inventoryView = new InventoryViewTestDataBuilder()
            .WithId(inventoryId)
            .WithQuantity(100)
            .WithUserId(42)
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

        OkObjectResult okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        okResult.Value.Should().NotBeNull();
        string jsonResult = (string)okResult.Value;
        jsonResult.Should().Contain("100");
        jsonResult.Should().NotContain("42");
        jsonResult.Should().NotContain("24");
    }
}
