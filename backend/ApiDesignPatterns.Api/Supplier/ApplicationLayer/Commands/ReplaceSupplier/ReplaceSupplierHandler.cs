// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.Supplier.InfrastructureLayer.Database.Supplier;

namespace backend.Supplier.ApplicationLayer.Commands.ReplaceSupplier;

public class ReplaceSupplierHandler(ISupplierRepository repository) : ICommandHandler<ReplaceSupplierCommand>
{
    public async Task Handle(ReplaceSupplierCommand command)
    {
        command.Supplier.Id = await repository.UpdateSupplierAsync(command.Supplier);
        await repository.UpdateSupplierAddressAsync(command.Supplier);
        await repository.UpdateSupplierPhoneNumberAsync(command.Supplier);
    }
}
