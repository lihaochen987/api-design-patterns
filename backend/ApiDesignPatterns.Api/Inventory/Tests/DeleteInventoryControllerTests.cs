// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryById;
using backend.Inventory.Controllers;
using backend.Inventory.Tests.TestHelpers.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace backend.Inventory.Tests;

public class DeleteInventoryControllerTests : DeleteInventoryControllerTestBase
{
    [Fact]
    public async Task DeleteInventory_InventoryExists_ReturnsNoContent()
    {
        var inventory =
            new InventoryTestDataBuilder().WithId(123L)
                .Build();
        var request = new DeleteInventoryRequest();
        Repository.Add(inventory);
        DeleteInventoryController sut = DeleteInventoryController();

        ActionResult result = await sut.DeleteInventory(inventory.Id, request);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteInventory_InventoryDoesNotExist_ReturnsNotFound()
    {
        long nonExistentId = Fixture.Create<long>();
        var request = new DeleteInventoryRequest();
        DeleteInventoryController sut = DeleteInventoryController();

        ActionResult result = await sut.DeleteInventory(nonExistentId, request);

        result.Should().BeOfType<NotFoundResult>();
    }
}
