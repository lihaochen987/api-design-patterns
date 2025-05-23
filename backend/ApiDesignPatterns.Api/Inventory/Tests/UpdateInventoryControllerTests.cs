// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using AutoFixture;
using backend.Inventory.Controllers;
using backend.Inventory.DomainModels.ValueObjects;
using backend.Inventory.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace backend.Inventory.Tests;

public class UpdateInventoryControllerTests : UpdateInventoryControllerTestBase
{
    [Fact]
    public async Task UpdateInventory_WithExistingInventory_ShouldReturnOkWithUpdatedQuantity()
    {
        var existingInventory = new InventoryTestDataBuilder().WithId(1).WithQuantity(new Quantity(100)).Build();
        var request = new UpdateInventoryRequest { Quantity = 200m, FieldMask = ["quantity"] };
        Repository.Add(existingInventory);
        var sut = UpdateInventoryController();

        var actionResult = await sut.UpdateInventory(existingInventory.Id, request);

        actionResult.Result.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeOfType<UpdateInventoryResponse>();
        var response = okResult.Value as UpdateInventoryResponse;
        response!.Quantity.Should().Be("200");
    }

    [Fact]
    public async Task UpdateInventory_WithNonExistentInventory_ShouldReturnNotFound()
    {
        long nonExistentId = Fixture.Create<long>();
        var request = Fixture.Create<UpdateInventoryRequest>();
        var sut = UpdateInventoryController();

        var actionResult = await sut.UpdateInventory(nonExistentId, request);

        actionResult.Result.Should().NotBeNull();
        actionResult.Result.Should().BeOfType<NotFoundResult>();
        (actionResult.Result as NotFoundResult)?.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task UpdateInventory_WithRestockDate_ShouldReturnOkWithUpdatedRestockDate()
    {
        var existingInventory = new InventoryTestDataBuilder().WithId(1).Build();
        var newRestockDate = DateTimeOffset.Now.AddDays(30);
        var request = new UpdateInventoryRequest
        {
            RestockDate = newRestockDate,
            FieldMask = ["restockdate"]
        };
        Repository.Add(existingInventory);
        var sut = UpdateInventoryController();

        var actionResult = await sut.UpdateInventory(existingInventory.Id, request);

        actionResult.Result.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult.Result as OkObjectResult;
        var response = okResult!.Value as UpdateInventoryResponse;
        response!.RestockDate.Should().Be(newRestockDate.ToString());
    }

    [Fact]
    public async Task UpdateInventory_WithMultipleFields_ShouldUpdateAllSpecifiedFields()
    {
        var existingInventory = new InventoryTestDataBuilder().WithId(1).WithQuantity(new Quantity(250)).Build();
        var newRestockDate = DateTimeOffset.Now.AddDays(15);
        var request = new UpdateInventoryRequest
        {
            Quantity = 250m,
            RestockDate = newRestockDate,
            FieldMask = ["quantity", "restockdate"]
        };
        Repository.Add(existingInventory);
        var sut = UpdateInventoryController();

        var actionResult = await sut.UpdateInventory(existingInventory.Id, request);

        actionResult.Result.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult.Result as OkObjectResult;
        var response = okResult!.Value as UpdateInventoryResponse;
        response!.Quantity.Should().Be("250");
        response.RestockDate.Should().Be(newRestockDate.ToString());
    }

    [Fact]
    public async Task UpdateInventory_WithPartialFieldMask_ShouldUpdateOnlySpecifiedFields()
    {
        var originalRestockDate = DateTimeOffset.Now.AddDays(10);
        var existingInventory = new InventoryTestDataBuilder()
            .WithId(1)
            .WithQuantity(new Quantity(100m))
            .WithRestockDate(originalRestockDate)
            .Build();
        var request = new UpdateInventoryRequest
        {
            Quantity = 300m,
            RestockDate = DateTimeOffset.Now.AddDays(20), // This should be ignored
            FieldMask = ["quantity"] // Only quantity should be updated
        };
        Repository.Add(existingInventory);
        var sut = UpdateInventoryController();

        var actionResult = await sut.UpdateInventory(existingInventory.Id, request);

        actionResult.Result.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult.Result as OkObjectResult;
        var response = okResult!.Value as UpdateInventoryResponse;
        response!.Quantity.Should().Be("300");
        response.RestockDate.Should().Be(originalRestockDate.ToString()); // Should remain unchanged
    }

    [Fact]
    public async Task UpdateInventory_WithEmptyFieldValues_ShouldNotUpdateFields()
    {
        decimal originalQuantity = Fixture.Create<decimal>();
        var originalRestockDate = DateTimeOffset.Now.AddDays(5);
        var existingInventory = new InventoryTestDataBuilder()
            .WithId(1)
            .WithQuantity(new Quantity(originalQuantity))
            .WithRestockDate(originalRestockDate)
            .Build();
        var request = new UpdateInventoryRequest
        {
            Quantity = null,
            RestockDate = null,
            FieldMask = ["quantity", "restockdate"]
        };
        Repository.Add(existingInventory);
        var sut = UpdateInventoryController();

        var actionResult = await sut.UpdateInventory(existingInventory.Id, request);

        actionResult.Result.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult.Result as OkObjectResult;
        var response = okResult!.Value as UpdateInventoryResponse;
        response!.Quantity.Should().Be(originalQuantity.ToString(CultureInfo.InvariantCulture)); // Should remain unchanged
        response.RestockDate.Should().Be(originalRestockDate.ToString()); // Should remain unchanged
    }
}
