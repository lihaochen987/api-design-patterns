// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.ApplicationLayer.Queries.GetInventoryView;
using backend.Inventory.DomainModels;
using backend.Inventory.Tests.TestHelpers.Builders;
using backend.Shared.QueryHandler;
using Shouldly;
using Xunit;

namespace backend.Inventory.Tests.ApplicationLayerTests;


public class GetInventoryViewHandlerTests : GetInventoryViewHandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsInventoryView_WhenInventoryExists()
    {
        InventoryView expectedInventory = new InventoryViewTestDataBuilder().Build();
        Repository.Add(expectedInventory);
        IQueryHandler<GetInventoryViewQuery, InventoryView> sut = GetInventoryViewHandler();

        InventoryView? result = await sut.Handle(new GetInventoryViewQuery { Id = expectedInventory.Id });

        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(expectedInventory);
        Repository.CallCount.ShouldContainKeyAndValue("GetInventoryView", 1);
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenInventoryDoesNotExist()
    {
        InventoryView nonExistentInventory = new InventoryViewTestDataBuilder().Build();
        IQueryHandler<GetInventoryViewQuery, InventoryView> sut = GetInventoryViewHandler();

        InventoryView? result = await sut.Handle(new GetInventoryViewQuery { Id = nonExistentInventory.Id });

        result.ShouldBeNull();
        Repository.CallCount.ShouldContainKeyAndValue("GetInventoryView", 1);
    }
}
