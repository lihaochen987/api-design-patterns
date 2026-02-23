// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.InfrastructureLayer.Database.Inventory;
using backend.Shared.CommandHandler;

namespace backend.Inventory.ApplicationLayer.Commands.UpdateInventory;

public class UpdateInventoryHandler(IInventoryRepository repository) : ICommandHandler<UpdateInventoryCommand>
{
    public async Task Handle(UpdateInventoryCommand command)
    {
        var req = command.Request;
        var current = command.Inventory;

        decimal quantity = req.FieldMask.Contains("quantity", StringComparer.OrdinalIgnoreCase) && req.Quantity != null
            ? req.Quantity ?? current.Quantity
            : current.Quantity;

        DateTimeOffset? restockDate = req.FieldMask.Contains("restockdate", StringComparer.OrdinalIgnoreCase) && req.RestockDate != null
            ? req.RestockDate ?? current.RestockDate
            : current.RestockDate;

        await repository.UpdateInventoryAsync(current with { Quantity = quantity, RestockDate = restockDate });
    }
}
