// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.InfrastructureLayer;
using backend.Shared.CommandHandler;

namespace backend.Product.ApplicationLayer.CreateProduct;

public class CreateProductHandler(IProductRepository repository) : ICommandHandler<CreateProduct>
{
    public async Task Handle(CreateProduct command)
    {
        await repository.CreateProductAsync(command.Product);
    }
}
