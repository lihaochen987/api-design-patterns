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

namespace backend.Inventory.Tests.ControllerTests;

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
        Mock.Get(GetInventoryByProductAndUser)
            .Setup(h => h.Handle(It.Is<GetInventoryByProductAndUserQuery>(q =>
                q.ProductId == request.ProductId && q.UserId == request.UserId)))
            .ReturnsAsync(existingInventory);
        CreateInventoryController sut = GetCreateInventoryController();

        var result = await sut.CreateInventory(request);

        result.Should().NotBeNull();
        result.Result.Should().BeOfType<ConflictResult>();
    }

    [Fact]
    public async Task CreateInventory_HandlesCommandFailure_WhenCreateInventoryFails()
    {
        var inventory = new InventoryTestDataBuilder().Build();
        var request = new CreateInventoryRequest
        {
            UserId = inventory.UserId,
            ProductId = inventory.ProductId,
            Quantity = inventory.Quantity.Value,
            RestockDate = inventory.RestockDate
        };
        Mock
            .Get(CreateInventory)
            .Setup(x => x.Handle(It.IsAny<CreateInventoryCommand>()))
            .ThrowsAsync(new Exception("Failed to create inventory"));
        var sut = GetCreateInventoryController();

        Func<Task> act = async () => await sut.CreateInventory(request);
        await act.Should().ThrowAsync<Exception>();
    }
}
