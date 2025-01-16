// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using backend.Product.InfrastructureLayer;
using backend.Shared;
using backend.Shared.CommandService;

namespace backend.Product.ApplicationLayer.CreateProduct;

public class CreateProductService(IProductRepository repository) : ICommandService<CreateProduct>
{
    public async Task Execute(CreateProduct command)
    {
        await repository.CreateProductAsync(command.Product);
    }
}
