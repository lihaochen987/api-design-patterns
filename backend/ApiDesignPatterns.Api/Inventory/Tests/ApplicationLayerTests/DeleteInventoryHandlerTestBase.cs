// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using AutoFixture;
using backend.Inventory.ApplicationLayer.Commands.DeleteInventory;
using backend.Inventory.Tests.TestHelpers.Fakes;
using backend.Shared.CommandHandler;

namespace backend.Inventory.Tests.ApplicationLayerTests;

public abstract class DeleteInventoryCommandHandlerTestBase
{
    protected readonly InventoryRepositoryFake Repository = [];
    protected readonly Fixture Fixture = new();

    protected ICommandHandler<DeleteInventoryCommand> DeleteInventoryHandler()
    {
        return new DeleteInventoryCommandHandler(Repository);
    }
}
