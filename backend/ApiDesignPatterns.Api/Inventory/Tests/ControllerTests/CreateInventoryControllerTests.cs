// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.ApplicationLayer.Commands.CreateInventory;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryByProductAndSupplier;
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
        var request = Mapper.Map<CreateInventoryRequest>(inventory);
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
        var request = Mapper.Map<CreateInventoryRequest>(existingInventory);
        Mock.Get(GetInventoryByProductAndSupplier)
            .Setup(h => h.Handle(It.Is<GetInventoryByProductAndSupplierQuery>(q =>
                q.ProductId == request.ProductId && q.SupplierId == request.SupplierId)))
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
        var request = Mapper.Map<CreateInventoryRequest>(inventory);
        Mock
            .Get(CreateInventory)
            .Setup(x => x.Handle(It.IsAny<CreateInventoryCommand>()))
            .ThrowsAsync(new Exception("Failed to create inventory"));
        var sut = GetCreateInventoryController();

        Func<Task> act = async () => await sut.CreateInventory(request);
        await act.Should().ThrowAsync<Exception>();
    }
}
