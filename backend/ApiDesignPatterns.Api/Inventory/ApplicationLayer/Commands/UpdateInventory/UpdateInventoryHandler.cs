// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.Controllers;
using backend.Inventory.InfrastructureLayer.Database.Inventory;
using backend.Shared.CommandHandler;

namespace backend.Inventory.ApplicationLayer.Commands.UpdateInventory;

public class UpdateInventoryHandler(IInventoryRepository repository) : ICommandHandler<UpdateInventoryCommand>
{
    public async Task Handle(UpdateInventoryCommand command)
    {
        (decimal quantity, DateTimeOffset? restockDate) = GetUpdatedInventoryValues(command.Request, command.Inventory);
        var inventory = command.Inventory with { Quantity = quantity, RestockDate = restockDate };
        await repository.UpdateInventoryAsync(inventory);
    }

    private static (
        decimal quantity,
        DateTimeOffset? restockDate)
        GetUpdatedInventoryValues(
            UpdateInventoryRequest request,
            DomainModels.Inventory inventory)
    {
        decimal quantity = request.FieldMask.Contains("quantity", StringComparer.OrdinalIgnoreCase)
                           && request.Quantity != null
            ? request.Quantity ?? inventory.Quantity
            : inventory.Quantity;

        DateTimeOffset? restockDate =
            request.FieldMask.Contains("restockdate", StringComparer.OrdinalIgnoreCase) && request.RestockDate != null
                ? request.RestockDate ?? inventory.RestockDate
                : inventory.RestockDate;

        return (quantity, restockDate);
    }
}
