// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Commands.CreateInventory;
using backend.Inventory.Tests.TestHelpers.Fakes;
using backend.Shared.CommandHandler;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public abstract class CreateInventoryHandlerTestBase
{
    protected readonly Fixture Fixture = new();
    protected readonly InventoryRepositoryFake Repository = [];

    protected ICommandHandler<CreateInventoryCommand> CreateInventoryService()
    {
        return new CreateInventoryHandler(Repository);
    }
}
