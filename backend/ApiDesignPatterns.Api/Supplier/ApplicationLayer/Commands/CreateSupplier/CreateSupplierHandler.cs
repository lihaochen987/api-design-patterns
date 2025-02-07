// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Shared.CommandHandler;
using backend.Supplier.InfrastructureLayer.Database.Supplier;

namespace backend.Supplier.ApplicationLayer.Commands.CreateSupplier;

public class CreateSupplierHandler(ISupplierRepository repository) : ICommandHandler<CreateSupplierCommand>
{
    public async Task Handle(CreateSupplierCommand command)
    {
        long id = await repository.CreateSupplierAsync(command.Supplier);
        var supplier = new DomainModels.Supplier
        {
            Id = id,
            FirstName = command.Supplier.FirstName,
            LastName = command.Supplier.LastName,
            Email = command.Supplier.Email,
            Address = command.Supplier.Address,
            CreatedAt = DateTimeOffset.UtcNow,
            PhoneNumber = command.Supplier.PhoneNumber,
        };
        await repository.CreateSupplierAddressAsync(supplier);
        await repository.CreateSupplierPhoneNumberAsync(supplier);
    }
}
