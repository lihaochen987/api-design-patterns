// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Commands.UpdateInventory;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryById;
using backend.Inventory.Controllers;
using backend.Inventory.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace backend.Inventory.Tests.ControllerTests;

public class UpdateInventoryControllerTests : UpdateInventoryControllerTestBase
{
    [Fact]
    public async Task UpdateInventory_WithExistingInventory_ShouldReturnOkWithMappedResponse()
    {
        var existingInventory = new InventoryTestDataBuilder().Build();
        var updatedInventoryState = new InventoryTestDataBuilder().WithId(existingInventory.Id).Build();
        var request = Fixture.Create<UpdateInventoryRequest>();
        Mock.Get(MockGetInventoryHandler)
            .SetupSequence(svc => svc.Handle(It.Is<GetInventoryByIdQuery>(q => q.Id == existingInventory.Id)))
            .ReturnsAsync(existingInventory)
            .ReturnsAsync(updatedInventoryState);
        var sut = UpdateInventoryController();
        var expectedResponse = Mapper.Map<UpdateInventoryResponse>(updatedInventoryState);

        var actionResult = await sut.UpdateInventory(existingInventory.Id, request);

        actionResult.Result.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeOfType<UpdateInventoryResponse>();
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task UpdateInventory_WithNonExistentInventory_ShouldReturnNotFound()
    {
        long nonExistentId = Fixture.Create<long>();
        var request = Fixture.Create<UpdateInventoryRequest>();
        Mock.Get(MockGetInventoryHandler)
            .Setup(svc => svc.Handle(It.Is<GetInventoryByIdQuery>(q => q.Id == nonExistentId)))
            .ReturnsAsync((DomainModels.Inventory?)null);
        var sut = UpdateInventoryController();

        var actionResult = await sut.UpdateInventory(nonExistentId, request);

        actionResult.Result.Should().NotBeNull();
        actionResult.Result.Should().BeOfType<NotFoundResult>();
        (actionResult.Result as NotFoundResult)?.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task UpdateInventory_WhenUpdateIsCalled_ShouldRefetchInventoryForResponse()
    {
        var existingInventory = new InventoryTestDataBuilder().Build();
        var updatedInventoryState = new InventoryTestDataBuilder().WithId(existingInventory.Id).Build();
        var request = Fixture.Create<UpdateInventoryRequest>();
        Mock.Get(MockGetInventoryHandler)
            .SetupSequence(svc => svc.Handle(It.Is<GetInventoryByIdQuery>(q => q.Id == existingInventory.Id)))
            .ReturnsAsync(existingInventory)
            .ReturnsAsync(updatedInventoryState);
        Mock.Get(MockUpdateInventoryHandler)
            .Setup(svc => svc.Handle(It.IsAny<UpdateInventoryCommand>()))
            .Returns(Task.CompletedTask);
        var sut = UpdateInventoryController();
        var expectedResponse = Mapper.Map<UpdateInventoryResponse>(updatedInventoryState);

        var actionResult = await sut.UpdateInventory(existingInventory.Id, request);

        actionResult.Result.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult.Result as OkObjectResult;
        okResult?.Value.Should().BeEquivalentTo(expectedResponse);
    }
}
