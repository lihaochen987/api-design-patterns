// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.Supplier.InfrastructureLayer.Database.Supplier;

namespace backend.Supplier.ApplicationLayer.Commands.DeleteSupplier;

public class DeleteSupplierHandler(ISupplierRepository repository) : ICommandHandler<DeleteSupplierCommand>
{
    public async Task Handle(DeleteSupplierCommand command)
    {
        await repository.DeleteSupplierAsync(command.Id);
    }
}
