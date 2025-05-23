// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.ApplicationLayer.Commands.CreateInventory;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryByProductAndUser;
using backend.Inventory.Controllers;
using backend.Inventory.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace backend.Inventory.Tests;

public class CreateInventoryControllerTests : CreateInventoryControllerTestBase
{
    [Fact]
    public async Task CreateInventory_ReturnsCreatedResponse_WhenInventoryCreatedSuccessfully()
    {
        var inventory = new InventoryTestDataBuilder().Build();
        var request = new CreateInventoryRequest
        {
            UserId = inventory.UserId,
            ProductId = inventory.ProductId,
            Quantity = inventory.Quantity.Value,
            RestockDate = inventory.RestockDate
        };
        CreateInventoryController sut = GetCreateInventoryController();

        var result = await sut.CreateInventory(request);

        result.Should().NotBeNull();
        Repository.IsDirty.Should().BeTrue();
        var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.ActionName.Should().Be("GetInventory");
        createdResult.ControllerName.Should().Be("GetInventory");
    }

    [Fact]
    public async Task CreateInventory_ReturnsConflict_WhenInventoryAlreadyExists()
    {
        var existingInventory = new InventoryTestDataBuilder().Build();
        var request = new CreateInventoryRequest
        {
            UserId = existingInventory.UserId,
            ProductId = existingInventory.ProductId,
            Quantity = existingInventory.Quantity.Value,
            RestockDate = existingInventory.RestockDate
        };
        Repository.Add(existingInventory);
        CreateInventoryController sut = GetCreateInventoryController();

        var result = await sut.CreateInventory(request);

        result.Should().NotBeNull();
        result.Result.Should().BeOfType<ConflictResult>();
    }
}
