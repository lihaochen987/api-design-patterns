// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Inventory.Controllers;
using backend.Inventory.DomainModels.ValueObjects;
using backend.Inventory.InfrastructureLayer.Database.Inventory;
using backend.Shared.CommandHandler;

namespace backend.Inventory.ApplicationLayer.Commands.UpdateInventory;

public class UpdateInventoryHandler(IInventoryRepository repository) : ICommandHandler<UpdateInventoryCommand>
{
    public async Task Handle(UpdateInventoryCommand command)
    {
        (Quantity quantity, DateTimeOffset? restockDate) =
            GetUpdatedInventoryValues(command.Request, command.Inventory);
        var inventory = command.Inventory with { Quantity = quantity, RestockDate = restockDate };
        await repository.UpdateInventoryAsync(inventory);
    }

    private static (
        Quantity quantity,
        DateTimeOffset? restockDate)
        GetUpdatedInventoryValues(
            UpdateInventoryRequest request,
            DomainModels.Inventory inventory)
    {
        Quantity quantity = GetUpdatedQuantity(request, inventory.Quantity.Value);

        DateTimeOffset? restockDate =
            request.FieldMask.Contains("restockdate", StringComparer.OrdinalIgnoreCase) && request.RestockDate != null
                ? request.RestockDate ?? inventory.RestockDate
                : inventory.RestockDate;

        return (quantity, restockDate);
    }

    private static Quantity GetUpdatedQuantity(
        UpdateInventoryRequest request,
        decimal currentQuantity)
    {
        decimal value = request.FieldMask.Contains("quantity", StringComparer.OrdinalIgnoreCase)
                        && request.Quantity != null
            ? request.Quantity ?? currentQuantity
            : currentQuantity;

        return new Quantity(value);
    }
}
