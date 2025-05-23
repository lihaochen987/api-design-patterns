// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Commands.DeleteInventory;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryById;
using backend.Inventory.Controllers;
using backend.Inventory.Tests.TestHelpers.Fakes;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Inventory.Tests;

public abstract class DeleteInventoryControllerTestBase
{
    protected readonly Fixture Fixture = new();

    protected readonly InventoryRepositoryFake Repository = [];

    protected DeleteInventoryController DeleteInventoryController()
    {
        var getInventoryByIdHandler = new GetInventoryByIdByIdHandler(Repository);
        var deleteInventoryHandler = new DeleteInventoryCommandHandler(Repository);
        return new DeleteInventoryController(getInventoryByIdHandler, deleteInventoryHandler);
    }
}
