// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.InfrastructureLayer.Database.Inventory;
using backend.Shared.CommandHandler;

namespace backend.Inventory.ApplicationLayer.Commands.CreateInventory;

public class CreateInventoryHandler(IInventoryRepository repository) : ICommandHandler<CreateInventoryCommand>
{
    public async Task Handle(CreateInventoryCommand command)
    {
        await repository.CreateInventoryAsync(command.Inventory);
    }
}
