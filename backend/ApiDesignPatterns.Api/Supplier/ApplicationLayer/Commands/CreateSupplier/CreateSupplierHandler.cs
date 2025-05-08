// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.Supplier.InfrastructureLayer.Database.Supplier;

namespace backend.Supplier.ApplicationLayer.Commands.CreateSupplier;

public class CreateSupplierHandler(ISupplierRepository repository) : ICommandHandler<CreateSupplierCommand>
{
    public async Task Handle(CreateSupplierCommand command)
    {
        var supplier = command.Supplier with { CreatedAt = DateTimeOffset.UtcNow };
        await repository.CreateSupplierAsync(supplier);
    }
}
