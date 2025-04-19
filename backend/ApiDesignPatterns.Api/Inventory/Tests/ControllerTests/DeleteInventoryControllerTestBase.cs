// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Commands.DeleteInventory;
using backend.Inventory.ApplicationLayer.Queries.GetInventoryById;
using backend.Inventory.Controllers;
using backend.Shared.CommandHandler;
using backend.Shared.QueryHandler;
using Moq;

namespace backend.Inventory.Tests.ControllerTests;

public abstract class DeleteInventoryControllerTestBase
{
    protected readonly Fixture Fixture = new();

    protected readonly IQueryHandler<GetInventoryByIdQuery, DomainModels.Inventory?> MockGetInventoryByIdHandler =
        Mock.Of<IQueryHandler<GetInventoryByIdQuery, DomainModels.Inventory?>>();

    protected readonly ICommandHandler<DeleteInventoryCommand> MockDeleteInventoryHandler =
        Mock.Of<ICommandHandler<DeleteInventoryCommand>>();

    protected DeleteInventoryController DeleteInventoryController()
    {
        return new DeleteInventoryController(MockGetInventoryByIdHandler, MockDeleteInventoryHandler);
    }
}
