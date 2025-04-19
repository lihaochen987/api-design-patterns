// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.InfrastructureLayer.Database.Inventory;
using backend.Shared.CommandHandler;

namespace backend.Inventory.ApplicationLayer.Commands.DeleteInventory;

public class DeleteInventoryCommandHandler(IInventoryRepository repository) : ICommandHandler<DeleteInventoryCommand>
{
    public async Task Handle(DeleteInventoryCommand command)
    {
        await repository.DeleteInventoryAsync(command.Id);
    }
}
