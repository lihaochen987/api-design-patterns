// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using backend.Product.InfrastructureLayer.Database.Product;
using backend.Shared.CommandHandler;

namespace backend.Product.ApplicationLayer.Commands.CreateProduct;

public class CreateProductHandler(ICreateProduct repository) : ICommandHandler<CreateProductCommand>
{
    public async Task Handle(CreateProductCommand command)
    {
        await repository.CreateProductAsync(command.Product);
    }
}
